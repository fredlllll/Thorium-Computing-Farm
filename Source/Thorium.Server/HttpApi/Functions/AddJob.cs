using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Thorium.Shared.DTOs.OperationData;
using Thorium.Shared.DTOs;
using Thorium.Shared;
using NLog;
using Thorium.Shared.FunctionServer.Http;

namespace Thorium.Server.HttpApi.Functions
{
    internal class AddJob : IHttpFunctionProvider
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly Dictionary<string, Type> operationTypeToType = new(){
            { "exe", typeof(ExeDTO)}
        };

        public string FunctionName => "addjob";

        public void Execute(HttpListenerContext context)
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
                op.OperationData = ((JsonElement)op.OperationData).Deserialize(type, JsonUtil.CaseInsensitive);
            }

            Database.ExecuteNonQuery("INSERT INTO jobs (id,name,description) VALUES (?,?,?)", jobData.Id, jobData.Name, jobData.Description);
            var queued = Shared.DTOs.TaskStatus.Queued.ToString();
            for (int i = 0; i < jobData.TaskCount; i++)
            {
                var id = Guid.NewGuid().ToString();
                Database.ExecuteNonQuery("INSERT INTO tasks (id,job_id,`index`,status) VALUES (?,?,?,?)", id, jobData.Id, i, queued);
            }
            for (int i = 0; i < jobData.Operations.Length; i++)
            {
                var op = jobData.Operations[i];
                var id = Guid.NewGuid().ToString();
                Database.ExecuteNonQuery("INSERT INTO operations (id,job_id,`index`,type) VALUES (?,?,?,?)", id, jobData.Id, i, op.OperationType);
                switch (op.OperationType)
                {
                    case "exe":
                        //"op_id", "file_path", "arguments", "working_dir"
                        var exeData = op.OperationData as ExeDTO;
                        Database.ExecuteNonQuery("INSERT INTO op_exes (op_id,file_path,arguments,working_dir) VALUES (?,?,?,?)", id, exeData.FilePath, exeData.Arguments, exeData.WorkingDir);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }


            context.Response.StatusCode = 200;
        }
    }
}
