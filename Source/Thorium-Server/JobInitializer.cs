﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NLog;
using Thorium_Shared;

namespace Thorium_Server
{
    [Serializable]
    public class JobInitializer : RestartableThreadClass
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public delegate void JobInitializedHandler(JobInitializer sender, Job job);
        public delegate void JobInitializationFailedHandler(JobInitializer sender, Job job, Exception ex);
        public event JobInitializedHandler JobInitialized;
        public event JobInitializationFailedHandler JobInitializationFailed;

        ConcurrentQueue<Job> initQueue = new ConcurrentQueue<Job>();
        bool running = false;
        private readonly ThoriumServer server;

        public JobInitializer(ThoriumServer server) : base(false)
        {
            this.server = server;
        }

        public override void Start()
        {
            running = true;
            base.Start();
        }

        public override void Stop()
        {
            running = false;
            base.Stop();
        }

        protected override void Run()
        {
            logger.Info("Initializer started");
            try
            {
                while(running)
                {
                    if(initQueue.TryDequeue(out Job job))
                    {
                        try
                        {
                            logger.Info("initializing job " + job.ID);
                            var producer = job.TaskProducer;
                            Task t;
                            while((t = producer.GetNextTask()) != null)
                            {
                                logger.Debug("got task: " + t.ID);
                                server.TaskManager.AddTask(t); //TODO: if you leave this in the release im going to murder you (thats me)
                                //TODO: save task to db
                            }
                            JobInitialized?.Invoke(this, job);
                            logger.Info("done");
                        }
                        //dont handle thread interrupt here
                        catch(Exception ex) when(!(ex is ThreadInterruptedException))
                        {
                            JobInitializationFailed?.Invoke(this, job, ex);
                        }
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch(ThreadInterruptedException)
            {
                //exiting thread
            }
            logger.Info("Initializer exited");
        }

        internal void AddJob(Job job)
        {
            initQueue.Enqueue(job);
        }
    }
}