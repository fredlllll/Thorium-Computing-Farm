using NLog;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Thorium.Shared.Aether;

namespace Thorium.Shared.Net.Tcp
{
    public class TcpConnectionServer
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly TcpListener listener;
        private readonly byte[] handshake;

        public int HandshakeReadTimeout { get; set; } = 3000;
        public int HandshakeWriteTimeout { get; set; } = 3000;


        public event EventHandler<TcpClient> ClientHandshakeSucceeded;
        public event EventHandler<TcpConnectionServerClient> NewClient;
        public event EventHandler<TcpClient> ClientHandshakeFailed;

        public AetherSerializerLibrary SerializerLibrary { get; set; } = null;

        public TcpConnectionServer(IPEndPoint endpoint, byte[] handshake)
        {
            listener = new TcpListener(endpoint);
            this.handshake = handshake;
        }

        public void Start()
        {
            if (SerializerLibrary != null)
            {
                if (!SerializerLibrary.ContainsSerializerForType(typeof(Message)))
                {
                    SerializerLibrary.Add(new MessageSerializer());
                }
            }
            else
            {
                SerializerLibrary = new AetherSerializerLibrary();
                SerializerLibrary.Add(new MessageSerializer());
            }

            listener.Start();
            listener.BeginAcceptTcpClient(AcceptClient, this);
        }

        public void Stop()
        {
            listener.Stop();
        }

        private bool CheckHandshake(TcpClient client)
        {
            var stream = client.GetStream();
            byte[] buffer = new byte[handshake.Length];
            var originalReadTimeout = stream.ReadTimeout;
            var originalWriteTimeout = stream.WriteTimeout;
            try
            {
                stream.ReadTimeout = HandshakeReadTimeout;
                stream.WriteTimeout = HandshakeWriteTimeout;
                try
                {
                    stream.ReadExactly(buffer, 0, buffer.Length);
                }
                catch (IOException)
                {
                    logger.Info("Failed reading handshake from client " + client.Client.RemoteEndPoint);
                    return false;
                }

                bool handshakeMatches = buffer.SequenceEqual(handshake);
                if (handshakeMatches)
                {
                    //write handshake back on success
                    try
                    {
                        stream.Write(handshake, 0, handshake.Length);
                    }
                    catch (IOException)
                    {
                        logger.Info("Failed writing back handshake to client " + client.Client.RemoteEndPoint);
                        return false;
                    }
                    logger.Info("Handshake success from client " + client.Client.RemoteEndPoint);
                    return true;
                }
                logger.Info("Handshake didn't match from client " + client.Client.RemoteEndPoint);
                return false;
            }
            finally
            {
                stream.ReadTimeout = originalReadTimeout;
                stream.WriteTimeout = originalWriteTimeout;
            }
        }

        private void AcceptClient(IAsyncResult asyncResult)
        {
            var client = listener.EndAcceptTcpClient(asyncResult);

            logger.Info("Tcp client connected from " + client.Client.RemoteEndPoint);

            if (CheckHandshake(client))
            {
                ClientHandshakeSucceeded?.Invoke(this, client);
                var serverClient = new TcpConnectionServerClient(this, client);
                serverClient.SerializerLibrary = SerializerLibrary;
                NewClient?.Invoke(this, serverClient);
                serverClient.Start();
            }
            else
            {
                ClientHandshakeFailed?.Invoke(this, client);
                client.Close();
            }

            listener.BeginAcceptTcpClient(AcceptClient, this);
        }
    }
}
