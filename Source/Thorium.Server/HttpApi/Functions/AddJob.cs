using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using Thorium.Shared.DTOs.OperationData;
using Thorium.Shared.DTOs;
using NLog;
using Thorium.Shared.FunctionServer.Http;
using Thorium.Shared.Util;
using Thorium.Shared.Database.Models;
using Thorium.Shared.Database;
using Microsoft.Extensions.DependencyInjection;

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
            var jobData = JsonSerializer.Deserialize<JobDTO>(content, Json.CaseInsensitive);
            if(jobData == null)
            {
                throw new Exception("job data is null");
            }

            Job newJob = new Job()
            {
                Id = DatabaseContext.GetNewId<Job>(),
                Name = jobData.Name,
                Description = jobData.Description
            };

            using var db = ThoriumServer.GetNewDb();
            db.Jobs.Add(newJob);
            for (int i = 0; i < jobData.TaskCount; i++)
            {
                var t = new Task()
                {
                    Id = DatabaseContext.GetNewId<Task>(),
                    JobId = newJob.Id,
                    Status = TaskStatus.Queued,
                    TaskNumber = i,
                    WasSuccessful = false,
                };
                db.Tasks.Add(t);
            }

            for (int i = 0; i < jobData.Operations.Length; i++)
            {
                var opData = jobData.Operations[i];
                var op = new Operation()
                {
                    Id = DatabaseContext.GetNewId<Operation>(),
                    JobId = newJob.Id,
                    OperationIndex = i,
                    Type = opData.OperationType,
                    Data = opData.OperationData
                };
                db.Operations.Add(op);
            }
            db.SaveChanges();

            context.Response.StatusCode = 200;
        }
    }
}
