using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using Thorium.Shared;
using Thorium.Shared.DTOs;
using Thorium.Shared.Messages;

namespace Thorium.Client
{
    public class ThoriumServerApi : IDisposable
    {
        private FunctionClient client;

        public void Start()
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect(Settings.Get<string>("serverInterface"), Settings.Get<int>("serverPort"));

            client = new FunctionClient(tcpClient, Encoding.ASCII.GetBytes("THOR"));
            client.Start();
        }

        public void Register(string clientId)
        {
            client.RemoteFunctionCall<object>("Register", false, clientId);
        }

        public ThoriumTask GetNextTask()
        {
            return client.RemoteFunctionCall<ThoriumTask>("GetNextTask", true);
        }

        public ThoriumJob GetJob(string id)
        {
            return client.RemoteFunctionCall<ThoriumJob>("GetJob", true, id);
        }

        public void TurnInTask(string jobId, int taskNumber, string state) //TODO: replace state with an enum
        {
            client.RemoteFunctionCall<object>("TurnInTask", false, jobId, taskNumber, state);
        }

        public void Heartbeat()
        {

        }

        public void Dispose()
        {
            //TODO: disconnect
        }
    }
}
