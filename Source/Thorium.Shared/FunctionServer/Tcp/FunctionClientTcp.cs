using NLog;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Thorium.Shared.Aether;
using Thorium.Shared.Aether.AetherSerializers;
using Thorium.Shared.Messages;

namespace Thorium.Shared.FunctionServer.Tcp
{
    public class FunctionClientTcp : IDisposable
    {
        public FunctionCallerTcp FunctionCaller { get; private set; }

        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly string host;
        private readonly int port;
        private TcpClient client;
        private NetworkStream stream;
        private readonly byte[] handshake;

        private Thread runThread;
        private bool running = false;

        public AetherStream Aether { get; private set; }

        public bool Connected { get { return client.Connected; } }

        public event EventHandler OnClose;

        public FunctionClientTcp(string host, int port, byte[] handshake)
        {
            this.host = host;
            this.port = port;
            this.handshake = handshake;
        }

        private bool HandshakeSuccessful()
        {
            stream.Write(handshake, 0, handshake.Length);
            byte[] buffer = new byte[handshake.Length];
            stream.ReadExactly(buffer, 0, buffer.Length); //TODO: timeout
            return buffer.SequenceEqual(handshake);
        }

        public void Start()
        {
            if (!running)
            {
                client = new TcpClient();
                client.Connect(host, port);
                stream = client.GetStream();
                if (!HandshakeSuccessful())
                {
                    client.Close();
                    throw new Exception("Handshake unsuccessful");
                }
                else
                {
                    Aether = new AetherStream(stream);
                    Aether.SerializerLibrary.Add(new FunctionCallSerializer());
                    Aether.SerializerLibrary.Add(new FunctionCallAnswerSerializer());

                    FunctionCaller = new FunctionCallerTcp(Aether);

                    runThread = new Thread(Run);
                    running = true;
                    runThread.Start();
                }
            }
        }

        public void Stop()
        {
            if (running)
            {
                running = false;
                runThread.Interrupt();
                runThread.Join();
                FunctionCaller.Stop();
                OnClose?.Invoke(this, null);
            }
        }

        private void Run()
        {
            while (running)
            {
                try
                {
                    object obj = Aether.Read();
                    if (obj is FunctionCallAnswer answer)
                    {
                        FunctionCaller.HandleFunctionCallAnswer(answer);
                    }
                }
                catch (IOException)
                {
                    OnClose?.Invoke(this, null);
                    break;
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
                catch(Exception ex)
                {
                    logger.Error(ex, "Error in read loop");
                }
            }
            FunctionCaller.Stop();
        }

        public void Dispose()
        {
            Stop();
            client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
