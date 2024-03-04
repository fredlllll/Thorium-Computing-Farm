using NLog;
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
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private object clientLock = new();
        private FunctionClientTcp client;

        public void Start()
        {
            EstablishConnection();
        }

        void EstablishConnection()
        {
            lock (clientLock)
            {
                while (true)
                {
                    client = new FunctionClientTcp(Settings.Get<string>("serverInterface"), Settings.Get<int>("serverPort"), Encoding.ASCII.GetBytes("THOR"));
                    try
                    {
                        client.Start();
                        client.OnClose += Client_OnClose;
                        return;
                    }
                    catch (SocketException)
                    {
                        //connection failed
                        logger.Warn("Connection failed, retrying");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        private void Client_OnClose(object sender, EventArgs e)
        {
            EstablishConnection();
        }

        private T CallWithRetry<T>(string functionName, bool needsAnswer, params object[] args)
        {
            while (true)
            {
                try
                {
                    lock (clientLock)
                    {
                        return client.RemoteFunctionCall<T>(functionName, needsAnswer, 5000, args);
                    }
                }
                catch (TimeoutException)
                {
                    Thread.Sleep(100);
                }
            }
        }

        public void Register(string clientId)
        {
            CallWithRetry<object>("Register", false, clientId);
        }

        public TaskDTO GetNextTask()
        {
            return CallWithRetry<TaskDTO>("GetNextTask", true);
        }

        public JobDTO GetJob(string id)
        {
            return CallWithRetry<JobDTO>("GetJob", true, id);
        }

        public void TurnInTask(string taskId, string reason)
        {
            //TODO: add reason
            CallWithRetry<object>("TurnInTask", false, taskId);
        }

        public void Heartbeat()
        {
            lock (clientLock)
            {
            }
        }

        public void Dispose()
        {
            lock (clientLock)
            {
                client.Stop();
            }
        }
    }
}
