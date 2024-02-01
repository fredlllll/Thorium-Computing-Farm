using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Thorium.Shared;
using Thorium.Shared.DTOs;
using Thorium.Shared.Messages;

namespace Thorium.Server
{
    public class ThoriumClient
    {
        public string Id { get; private set; }

        private readonly FunctionServer functionServer;

        private readonly ThoriumServer server;

        public DateTime LastHeartbeat { get; private set; }

        public ThoriumClient(TcpClient client, ThoriumServer server)
        {
            this.functionServer = new FunctionServer(client, Encoding.ASCII.GetBytes("THOR"));
            this.server = server;
        }

        public void Start()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;

            var t = GetType();

            functionServer.AddFunction(t.GetMethod(nameof(Register), flags), this);
            functionServer.AddFunction(t.GetMethod(nameof(Heartbeat), flags), this);
            functionServer.AddFunction(t.GetMethod(nameof(GetNextTask), flags), this);
            functionServer.AddFunction(t.GetMethod(nameof(GetJob), flags), this);
            functionServer.AddFunction(t.GetMethod(nameof(TurnInTask), flags), this);
            functionServer.AddFunction(t.GetMethod(nameof(Log), flags), this);
            functionServer.Start();
        }

        #region functions

        void Register(string id)
        {
            Id = id;
        }

        void Heartbeat()
        {
            LastHeartbeat = DateTime.Now;
        }

        ThoriumTask GetNextTask()
        {
            var t = server.GetTask();
            if (t == null)
            {
                return null;
            }
            return t.ToThoriumTask();
        }

        ThoriumJob GetJob(string id)
        {
            return server.GetJob(id)?.ThoriumJob;
        }

        void TurnInTask(string jobId, int taskNumber, string status)
        {
            var job = server.GetJob(jobId);
            if (job != null)
            {
                job.TurnInTask(taskNumber, status);
            }
        }

        void Log(string msg)
        {
            //TODO
        }
        #endregion
    }
}
