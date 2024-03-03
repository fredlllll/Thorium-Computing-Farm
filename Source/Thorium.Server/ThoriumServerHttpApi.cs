using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Thorium.Shared;
using Thorium.Shared.DTOs.OperationData;
using Thorium.Shared.DTOs;
using NLog;
using Thorium.Server.Models;

namespace Thorium.Server
{
    public class ThoriumServerHttpApi
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly Dictionary<string, Type> operationTypeToType = new(){
            { "exe", typeof(ExeDTO)}
        };
        private readonly FunctionServerHttp api;

        public ThoriumServerHttpApi()
        {
            var apiListener = new HttpListener();
            apiListener.Prefixes.Add("http://*:" + Settings.Get<int>("httpApiPort") + "/");

            api = new FunctionServerHttp(apiListener);

            api.AddFunction("addjob", AddJob);
            api.AddFunction("listjobs", ListJobs);
        }

        public void Start()
        {
            api.Start();
        }

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
            var jobData = JsonSerializer.Deserialize<JobDTO>(content, JsonUtil.CaseInsensitive);
            //convert operation data to proper type
            foreach (var op in jobData.Operations)
            {
                var type = operationTypeToType[op.OperationType];
                op.OperationData = JsonSerializer.Deserialize((JsonElement)op.OperationData, type, JsonUtil.CaseInsensitive);
            }
            var job = JobUtil.CreateFromDTO(jobData);

            context.Response.StatusCode = 200;
        }

        public void ListJobs(HttpListenerContext context)
        {
            if (context.Request.HttpMethod != "GET")
            {
                context.Response.StatusCode = 400;
                return;
            }

            //var content = JsonSerializer.Serialize(database.jobs);
            context.Response.StatusCode = 200;
            using var sw = new StreamWriter(context.Response.OutputStream);
            //sw.Write(content);
        }
    }
}
