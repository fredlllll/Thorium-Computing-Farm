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
                Created = DateTime.Now,
                Updated = DateTime.Now,
                Name = jobData.Name,
                Description = jobData.Description
            };

            var db = DI.ServiceProvider.GetRequiredService<DatabaseContext>();
            db.Jobs.Add(newJob);
            List<Task> newTasks = new();
            for (int i = 0; i < jobData.TaskCount; i++)
            {
                var t = new Task()
                {
                    Id = DatabaseContext.GetNewId<Task>(),
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Job = newJob,
                    Status = TaskStatus.Queued,
                    TaskNumber = i,
                    WasSuccessful = false,
                };
                newTasks.Add(t);
            }
            db.Tasks.AddRange(newTasks);

            for (int i = 0; i < jobData.Operations.Length; i++)
            {
                var opData = jobData.Operations[i];
                var op = new Operation()
                {
                    Id = DatabaseContext.GetNewId<Operation>(),
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Job = newJob,
                    OperationIndex = i,
                    Type = opData.OperationType,
                    Data = opData.OperationData
                };
            }

            context.Response.StatusCode = 200;
        }
    }
}
