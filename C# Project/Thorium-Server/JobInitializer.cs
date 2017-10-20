using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thorium_Shared;
using static Thorium_Shared.SharedStatics;

namespace Thorium_Server
{
    [Serializable]
    public class JobInitializer : RestartableThreadClass
    {
        public delegate void JobInitializedHandler(JobInitializer sender, AJob job);
        public delegate void JobInitializationFailedHandler(JobInitializer sender, AJob job, Exception ex);
        public event JobInitializedHandler JobInitialized;
        public event JobInitializationFailedHandler JobInitializationFailed;

        ConcurrentQueue<AJob> initQueue = new ConcurrentQueue<AJob>();
        bool running = false;

        public JobInitializer() : base(false)
        {
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
            try
            {
                while(running)
                {
                    AJob job = default(AJob);
                    if(initQueue.TryDequeue(out job))
                    {
                        try
                        {
                            Logger.Log("initializing job " + job.ID);
                            var producer = job.TaskProducer;
                            Task t;
                            while((t = producer.GetNextTask()) != null)
                            {
                                //TODO: save task to db
                            }
                            JobInitialized?.Invoke(this, job);
                            Logger.Log("done");
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
        }

        internal void AddJob(AJob job)
        {
            initQueue.Enqueue(job);
        }
    }
}
