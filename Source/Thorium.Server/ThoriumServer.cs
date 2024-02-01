using System.Threading;
using Thorium.Shared;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using NLog;
using System.Net;
using System.Reflection;
using System.IO;
using System.Text.Json;
using Thorium.Shared.DTOs;

namespace Thorium.Server
{
    public class ThoriumServer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        readonly List<ThoriumClient> clients = [];

        TcpListener listener;
        FunctionServerHttp api;

        Dictionary<string, Job> jobs = new();

        public ThoriumServer()
        {
            //TODO: add api functions
        }

        public void StartListeners()
        {
            string _interface = Settings.Get<string>("clientInterface");
            int port = Settings.Get<int>("clientPort");
            logger.Info($"Starting to listen on {_interface}:{port}");
            listener = new TcpListener(IPAddress.Parse(_interface), port);
            listener.Start();
            listener.BeginAcceptTcpClient(HandleNewClient, null);

            var apiListener = new HttpListener();
            apiListener.Prefixes.Add("http://*:" + Settings.Get<int>("apiPort") + "/");
            api = new FunctionServerHttp(apiListener);

            api.AddFunction("addjob", AddJob);
            api.AddFunction("listjobs", ListJobs);

            api.Start();
        }

        private void HandleNewClient(IAsyncResult ar)
        {
            var tcpClient = listener.EndAcceptTcpClient(ar);
            var client = new ThoriumClient(tcpClient, this);

            client.Start();
            clients.Add(client);

            listener.BeginAcceptTcpClient(HandleNewClient, null);
        }


        public void Run()
        {
            while (true)
            {
                //TODO: i dont really know what to even do here
                Thread.Sleep(1000);
            }
        }

        public Task GetTask()
        {
            lock (jobs)
            {
                foreach (var kv in jobs)
                {
                    var job = kv.Value;
                    if (job.HasTasks)
                    {
                        return job.GetNextTask();
                    }
                }
            }
            return null;
        }

        public Job GetJob(string id)
        {
            if (jobs.TryGetValue(id, out var job))
            {
                return job;
            }
            return null;
        }

        #region apifunctions
        public void AddJob(HttpListenerContext context)
        {
            if (context.Request.HttpMethod != "POST" || !context.Request.HasEntityBody)
            {
                context.Response.StatusCode = 400;
                return;
            }
            var stream = context.Request.InputStream;
            StreamReader sr = new StreamReader(stream);
            var content = sr.ReadToEnd();

            logger.Info(content);
            var jobData = JsonSerializer.Deserialize<ThoriumJob>(content, JsonUtil.CaseInsensitive);
            var job = new Job(jobData);

            jobs[job.ThoriumJob.Id] = job;

            context.Response.StatusCode = 200;
        }

        public void ListJobs(HttpListenerContext context)
        {
            if (context.Request.HttpMethod != "GET")
            {
                context.Response.StatusCode = 400;
                return;
            }

            var content = JsonSerializer.Serialize(jobs);
            context.Response.StatusCode = 200;
            using var sw = new StreamWriter(context.Response.OutputStream);
            sw.Write(content);
        }
        #endregion
    }
}
