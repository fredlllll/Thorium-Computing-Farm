using System.Threading;
using Thorium.Shared;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using NLog;
using System.Net;
using System.Reflection;

namespace Thorium.Server
{
    public class ThoriumServer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        readonly Dictionary<string, MethodInfo> apiFunctions = new();
        readonly List<ThoriumClient> clients = [];

        HttpListener apiListener;
        TcpListener listener;

        public ThoriumServer()
        {
            //TODO: add api functions
        }

        public void StartListeners()
        {
            string _interface = Settings.Get<string>("clientInterface");
            int port = Settings.Get<int>("clientPort");
            logger.Info($"Starting to listen on {_interface}:{port}");
            listener = new TcpListener(System.Net.IPAddress.Parse(_interface), port);
            listener.Start();
            listener.BeginAcceptTcpClient(HandleNewClient, null);

            apiListener = new HttpListener();
            apiListener.Prefixes.Add("http://*:" + Settings.Get<int>("apiPort") + "/");
            apiListener.Start();
            apiListener.BeginGetContext(HandleNewApiRequest, null);
        }

        private void HandleNewClient(IAsyncResult ar)
        {
            var tcpClient = listener.EndAcceptTcpClient(ar);
            var client = new ThoriumClient(tcpClient, this);

            if (client.HandshakeSuccessful())
            {
                client.Start();
                clients.Add(client);
            }
            else
            {
                tcpClient.Close();
            }
            listener.BeginAcceptTcpClient(HandleNewClient, null);
        }

        private void HandleNewApiRequest(IAsyncResult ar)
        {
            var context = apiListener.EndGetContext(ar);
            var request = context.Request;
            var response = context.Response;

            var functionName = request.Url.AbsolutePath;


            apiListener.BeginGetContext(HandleNewApiRequest, null);
        }

        public void Run()
        {
            while (true)
            {
                //TODO: i dont really know what to even do here
                Thread.Sleep(1000);
            }
        }
    }
}
