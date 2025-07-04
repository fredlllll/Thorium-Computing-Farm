using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace Thorium.Shared.FunctionServer.Tcp
{
    public class FunctionServerTcp
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly TcpListener listener;
        private readonly byte[] handshake;

        private readonly List<FunctionServerTcpClient> clients = [];
        public int HandshakeReadTimeout { get; set; } = 3000;
        public int HandshakeWriteTimeout { get; set; } = 3000;

        public FunctionCallHandlerTcp FunctionCallHandler { get; } = new();

        public event EventHandler<TcpClient> ClientHandshakeSucceeded;
        public event EventHandler<FunctionServerTcpClient> ClientAdded;
        public event EventHandler<FunctionServerTcpClient> ClientRemoved;
        public event EventHandler<TcpClient> ClientHandshakeFailed;

        public FunctionServerTcp(TcpListener listener, byte[] handshake)
        {
            this.listener = listener;
            this.handshake = handshake;
        }

        public void Start()
        {
            listener.Start();
            listener.BeginAcceptTcpClient(AcceptClient, this);
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
                    return false;
                }

                bool clientHandshake = buffer.SequenceEqual(handshake);
                if (clientHandshake)
                {
                    //write handshake back on success
                    try
                    {
                        stream.Write(handshake, 0, handshake.Length);
                    }
                    catch (IOException)
                    {
                        return false;
                    }
                    return true;
                }
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

            if (CheckHandshake(client))
            {
                ClientHandshakeSucceeded?.Invoke(this, client);
                var serverClient = new FunctionServerTcpClient(this, client);
                ClientAdded?.Invoke(this, serverClient);
                serverClient.Start();
                clients.Add(serverClient);
            }
            else
            {
                ClientHandshakeFailed?.Invoke(this, client);
                client.Close();
            }

            listener.BeginAcceptTcpClient(AcceptClient, this);
        }

        internal void SelfRemoveClient(FunctionServerTcpClient client)
        {
            ClientRemoved?.Invoke(this, client);
            clients.Remove(client);
        }
    }
}
