using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium.Shared.DTOs.OperationData;
using Thorium.Shared;
using System.Net;
using System.Net.Sockets;
using Thorium.Shared.DTOs;
using System.Reflection;

namespace Thorium.Server
{
    public class ThoriumServerTcpApi
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly FunctionServerTcp api;

        public ThoriumServerTcpApi()
        {
            var apiListener = new TcpListener(IPAddress.Parse(Settings.Get<string>("tcpApiInterface")), Settings.Get<int>("tcpApiPort"));

            api = new FunctionServerTcp(apiListener, Encoding.ASCII.GetBytes("THOR"));

            var type = GetType();
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;

            api.AddFunction(type.GetMethod(nameof(Register), flags), this);
            api.AddFunction(type.GetMethod(nameof(Heartbeat), flags), this);
            api.AddFunction(type.GetMethod(nameof(GetNextTask), flags), this);
            api.AddFunction(type.GetMethod(nameof(GetJob), flags), this);
            api.AddFunction(type.GetMethod(nameof(TurnInTask), flags), this);
            api.AddFunction(type.GetMethod(nameof(Log), flags), this);
        }

        public void Start()
        {
            api.Start();
        }

        public Task GetNextTask(FunctionServerTcpClient client)
        {
            return null;
        }

        void Register(FunctionServerTcpClient client, string id)
        {
            Id = id;
        }

        void Heartbeat(FunctionServerTcpClient client)
        {
            LastHeartbeat = DateTime.Now;
        }

        TaskDTO GetNextTask(FunctionServerTcpClient client)
        {
            var t = server.GetTask();
            if (t == null)
            {
                return null;
            }
            return TaskUtil.ToDto(t);
        }

        JobDTO GetJob(FunctionServerTcpClient client, string id)
        {
            return JobUtil.ToDto(server.GetJob(id));
        }

        void TurnInTask(FunctionServerTcpClient client, string jobId, int taskNumber, string status)
        {
            var job = server.GetJob(jobId);
            if (job != null)
            {
                JobUtil.TurnInTask(job, taskNumber, status);
            }
        }

        void Log(FunctionServerTcpClient client, string msg)
        {
            //TODO
        }
    }
}
