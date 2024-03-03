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
        private FunctionClientTcp client;

        public void Start()
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect(Settings.Get<string>("serverInterface"), Settings.Get<int>("serverPort"));

            client = new FunctionClientTcp(tcpClient, Encoding.ASCII.GetBytes("THOR"));
            client.Start();
        }

        public void Register(string clientId)
        {
            client.RemoteFunctionCall<object>("Register", false, clientId);
        }

        public TaskDTO GetNextTask()
        {
            return client.RemoteFunctionCall<TaskDTO>("GetNextTask", true);
        }

        public JobDTO GetJob(string id)
        {
            return client.RemoteFunctionCall<JobDTO>("GetJob", true, id);
        }

        public void TurnInTask(string taskId, string reason)
        {
            //TODO: add reason
            client.RemoteFunctionCall<object>("TurnInTask", false, taskId);
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
