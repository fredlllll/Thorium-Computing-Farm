using NLog;
using System;
using System.Text;
using Thorium.Shared.DTOs;
using Thorium.Shared.FunctionServer.Tcp;

namespace Thorium.Client
{
    public class ThoriumServerApi : IDisposable
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private object clientLock = new();
        private FunctionClientTcp client;

        public string ServerHost { get; set; }
        public int ServerPort { get; set; }

        void EnsureConnection()
        {
            lock (clientLock)
            {
                if (client == null || !client.Connected)
                {
                    client = new FunctionClientTcp(ServerHost, ServerPort, Encoding.ASCII.GetBytes("THOR"));
                    client.Start();
                }
            }
        }

        private T Call<T>(string functionName, bool needsAnswer, params object[] args)
        {
            EnsureConnection();
            return client.FunctionCaller.RemoteFunctionCall<T>(functionName, needsAnswer, 5000, args);
        }

        public RegisterAnswer Register(string clientId, string clientName)
        {
            return Call<RegisterAnswer>("Register", true, clientId, clientName);
        }

        public void Dispose()
        {
            lock (clientLock)
            {
                client.Dispose();
            }
        }
    }
}
