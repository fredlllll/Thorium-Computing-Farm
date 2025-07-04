using System;
using System.Threading;
using NLog;
using Microsoft.Extensions.DependencyInjection;
using Thorium.Shared.Util;
using Thorium.Shared.Database;
using Microsoft.EntityFrameworkCore;
using Thorium.Shared.Database.Models;

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

            var options = new DbContextOptionsBuilder<DatabaseContext>().UseNpgsql(conn.GetConnectionString()).Options;
            DatabaseContext db = new DatabaseContext(options);
            DI.Services.AddSingleton(db);
            

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
                    if (currentJob == null || currentJob.Id != currentTask.Job.Id)
                    {
                        currentJob = currentTask.Job;
                        operations = new OperationList(currentJob);
                    }

                    logger.Info("got task: " + currentTask.Job.Id + ": " + currentTask.TaskNumber);

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
                logger.Info("exception");
                logger.Info(ex);
            }
            logger.Info("leaving worker thread");
        }

        private Task GetNextTask()
        {
            return null;
        }

        private void TurnInTask(Task task, string status)
        {

        }
    }
}
