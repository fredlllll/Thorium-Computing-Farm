using System;
using System.Threading;
using NLog;
using Microsoft.Extensions.DependencyInjection;
using Thorium.Shared.Util;
using Thorium.Shared.Database;
using Microsoft.EntityFrameworkCore;
using Thorium.Shared.Database.Models;
using System.Linq;

namespace Thorium.Client
{
    public class ThoriumClient
    {
        private string id = "unknown"; //TODO: save last one and load on startup
        private string name = System.Net.Dns.GetHostName();  //TODO. node specific string?

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Task? currentTask = null;
        private Job? currentJob = null;

        private OperationList? operations = null;

        public static DatabaseContext GetNewDb()
        {
            var conn = DI.ServiceProvider.GetRequiredService<DatabaseConnection>();
            var options = DatabaseContext.GetOptionsBuilder(conn.GetConnectionString()).Options;
            return new DatabaseContext(options);
        }

        private void FetchCurrentJob(string jobId)
        {
            using var db = GetNewDb();
            currentJob = db.Jobs.Where(x => x.Id == jobId).First();
            operations = new OperationList(currentJob);
        }

        public void Run()
        {
            ThoriumServerApi server = DI.ServiceProvider.GetRequiredService<ThoriumServerApi>();

            var registerInfo = server.Register(id,name);
            id = registerInfo.NodeId;

            //initialize data for database connections
            var conn = new DatabaseConnection()
            {
                Host = registerInfo.DatabaseHost,
                Port = registerInfo.DatabasePort,
                User = registerInfo.DatabaseUser,
                Password = registerInfo.DatabasePassword,
                Database = registerInfo.DatabaseName
            };
            DI.Services.AddSingleton<DatabaseConnection>(_=>conn);
            DI.ResetServiceProvider();

            var shit = DI.ServiceProvider.GetRequiredService<DatabaseConnection>();
            
            try
            {
                while (true)
                {
                    //TODO: wait for message from redis instead to check what task/job we have been assigned
                    logger.Info("getting task...");
                    currentTask = GetNextTask();
                    if (currentTask == null)
                    {
                        logger.Info("no task, waiting");
                        Thread.Sleep(1000);
                        continue;
                    }
                    //TODO: put these into a single call
                    if (currentJob == null || currentJob.Id != currentTask.JobId)
                    {
                        FetchCurrentJob(currentTask.JobId);
                    }

                    logger.Info("got task: " + currentTask.JobId + ": " + currentTask.TaskNumber);

                    try
                    {
                        logger.Info("executing task");
                        for (int i = 0; i < operations.operations.Count; ++i)
                        {
                            var op = operations.operations[i];
                            op.Execute(currentTask.TaskNumber);
                        }
                        TurnInTask(currentTask, "finished");
                        logger.Info("done task");
                    }
                    catch (Exception execEx) when (execEx is not ThreadInterruptedException)
                    {
                        logger.Info("task failed: " + execEx);
                        TurnInTask(currentTask, "error");
                    }
                    currentTask = null;
                }
            }
            catch (ThreadInterruptedException)
            {
                logger.Info("worker thread interrupted. exiting");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            logger.Info("leaving worker thread");
        }

        private Task? GetNextTask()
        {
            using var db = GetNewDb();
            var node = db.Nodes.Where(x => x.Id == id).First();

            var firstLinedUpTask = db.Tasks.Where(x => x.LinedUpOnNodeId == id).FirstOrDefault();
            if(firstLinedUpTask != null)
            {
                node.CurrentTaskId = firstLinedUpTask.Id;
                firstLinedUpTask.LinedUpOnNodeId = null;
                firstLinedUpTask.Status = TaskStatus.Running;
                db.SaveChanges();
                return firstLinedUpTask;
            }
            return null;
        }

        private void TurnInTask(Task task, string status)
        {
            using var db = GetNewDb();
        }
    }
}
