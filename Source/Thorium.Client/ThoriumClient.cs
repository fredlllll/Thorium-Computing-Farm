using System;
using System.Threading;
using NLog;
using Thorium.Shared;
using Microsoft.Extensions.DependencyInjection;
using Thorium.Shared.DTOs;

namespace Thorium.Client
{
    public class ThoriumClient
    {
        private string id = "machine_"; //TODO machine specific string

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private TaskDTO currentTask = null;
        private JobDTO currentJob = null;

        private OperationList operations = null;

        public void Run()
        {
            ThoriumServerApi server = DI.ServiceProvider.GetRequiredService<ThoriumServerApi>();

            server.Register(id);
            try
            {
                while (true)
                {
                    logger.Info("getting task...");
                    currentTask = server.GetNextTask();
                    if (currentTask == null)
                    {
                        logger.Info("no task, waiting");
                        Thread.Sleep(1000);
                        continue;
                    }
                    //TODO: put these into a single call
                    if (currentJob == null || currentJob.Id != currentTask.JobId)
                    {
                        currentJob = server.GetJob(currentTask.JobId);
                        operations = new OperationList(currentJob);
                    }

                    logger.Info("got task: " + currentTask.JobId + ": " + currentTask.TaskNumber);

                    try
                    {
                        logger.Info("executing task");
                        for (int i = 0; i < operations.operations.Length; ++i)
                        {
                            var op = operations.operations[i];
                            op.Execute(currentTask.TaskNumber);
                        }
                        server.TurnInTask(currentTask.Id,"finished");
                        logger.Info("done task");
                    }
                    catch (Exception execEx) when (execEx is not ThreadInterruptedException)
                    {
                        logger.Info("task failed: " + execEx);
                        server.TurnInTask(currentTask.Id,"error");
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
    }
}
