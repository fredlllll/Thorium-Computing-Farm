using NLog;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Thorium.Shared.Aether;
using Thorium.Shared.Util;

namespace Thorium.Shared.Net.Tcp
{
    public class TcpConnectionServerClient
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private bool running = false;
        private TcpConnectionServer server; //TODO: what do i need this for here?
        private TcpClient client;
        private NetworkStream stream;
        private AetherStream aether;
        private Thread runThread;
        private LimitedQueue<long> handledMessageIds = new(100);

        public string Id { get; private set; }
        public AetherSerializerLibrary SerializerLibrary { get; set; } = null;

        public event EventHandler<Message> MessageReceived;
        public event EventHandler Disconnected;

        public TcpConnectionServerClient(TcpConnectionServer server, TcpClient client)
        {
            this.server = server;
            this.client = client;
        }

        public void Start()
        {
            if (!running)
            {
                stream = client.GetStream();
                aether = new AetherStream(stream);
                aether.SerializerLibrary = SerializerLibrary;

                runThread = new Thread(Run);
                running = true;
                runThread.Start();
            }
        }

        public void Stop()
        {
            runThread.Interrupt();
            runThread.Join();
        }

        private void CloseOnDisconnect()
        {
            running = false;
            logger.Info("Client " + client.Client.RemoteEndPoint + " has gone away");
            Disconnected?.Invoke(this, null);
        }

        public void SendMessage(Message message)
        {
            aether.Write(message);
        }

        private void Run()
        {
            Id = aether.reader.ReadString();
            while (running)
            {
                Message message;
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
                    CloseOnDisconnect();
                    break;
                }
                catch (InvalidCastException ex)
                {
                    logger.Error(ex, "Received non message object");
                    continue;
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
        }
    }
}
