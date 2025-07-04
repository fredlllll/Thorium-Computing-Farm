using NLog;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Thorium.Shared.Aether;
using Thorium.Shared.Util;

namespace Thorium.Shared.Net.Tcp
{
    public class TcpConnectionClient
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IPEndPoint endpoint;
        private readonly byte[] handshake;
        private TcpClient client;
        private NetworkStream stream;
        private AetherStream aether;
        private bool running = false;
        private Thread readThread, writeThread;
        private readonly LimitedQueue<long> handledMessageIds = new(100);
        private readonly SemaphoreSlim messagesToSendSemaphore = new(0, int.MaxValue);
        private readonly ConcurrentQueue<Message> messagesToSend = new();
        private readonly object connectLock = new();

        public string Id { get; private set; }

        public event EventHandler<object> MessageReceived;

        public TcpConnectionClient(IPEndPoint endpoint, byte[] handshake, string id)
        {
            this.endpoint = endpoint;
            this.handshake = handshake;
            Id = id;
        }

        public void EnqueueMessage(Message message)
        {
            messagesToSend.Enqueue(message);
            messagesToSendSemaphore.Release();
        }

        private static bool HandshakeSuccessful(NetworkStream stream, byte[] handshake)
        {
            stream.Write(handshake, 0, handshake.Length);
            byte[] buffer = new byte[handshake.Length];
            stream.ReadExactly(buffer, 0, buffer.Length); //TODO: timeout
            return buffer.SequenceEqual(handshake);
        }

        private void Connect()
        {
            if (client != null)
            {
                return;
            }
            lock (connectLock)
            {
                while (running)
                {
                    try
                    {
                        client = new TcpClient();
                        client.Connect(endpoint);
                        stream = client.GetStream();
                    }
                    catch (SocketException)
                    {
                        //connection failed, retry
                        client = null;
                        continue;
                    }
                    try
                    {
                        if (!HandshakeSuccessful(stream, handshake))
                        {
                            client.Close();
                            client = null;
                            logger.Error("Handshake unsuccessful, id: " + Id + ", endpoint: " + endpoint);
                            running = false;
                            break;
                        }

                        aether = new AetherStream(stream);
                        aether.writer.Write(Id);
                        aether.SerializerLibrary.Add(new MessageSerializer());

                    }
                    catch (IOException)
                    {
                        //disconnect
                        client = null;
                    }
                }
            }
        }

        public void Start()
        {
            if (!running)
            {
                readThread = new Thread(RunRead);
                writeThread = new Thread(RunWrite);
                running = true;
                readThread.Start();
                writeThread.Start();
            }
        }

        public void Stop()
        {
            if (running)
            {
                running = false;
                readThread.Interrupt();
                writeThread.Interrupt();
                readThread.Join();
                writeThread.Join();
            }
        }

        private void RunRead()
        {
            while (running)
            {
                Connect();
                Message message = null;
                try
                {
                    message = (Message)aether.Read();
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
                catch (IOException)
                {
                    client = null;
                    continue;
                }
                catch (InvalidCastException ex)
                {
                    logger.Error(ex, "Received non message object");
                }

                if (!handledMessageIds.Contains(message.Id))
                {
                    handledMessageIds.Enqueue(message.Id);
                    try
                    {
                        MessageReceived?.Invoke(this, message);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Error handling received message " + message.Id);
                    }
                }
                else
                {
                    logger.Info("Ignoring duplicate message with id " + message.Id);
                }
            }
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }

        private void RunWrite()
        {
            while (running)
            {
                try
                {
                    messagesToSendSemaphore.Wait();
                    if (!messagesToSend.TryPeek(out Message message))
                    {
                        logger.Error("got semaphore, but couldnt peek queue?");
                        continue;
                    }
                    aether.Write(message);
                    if (!messagesToSend.TryDequeue(out message))
                    {
                        logger.Error("wasnt able to dequeue message after peeking it, impossible with only one thread reading?");
                    }
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
                catch (IOException)
                {
                    messagesToSendSemaphore.Release(); //message couldnt be sent, and thus wasnt dequeued
                    //solved by read thread
                }
            }
        }
    }
}
