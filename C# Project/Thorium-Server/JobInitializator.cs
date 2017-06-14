using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Server
{
    [Serializable]
    public class JobInitializator
    {
        public delegate void JobInitializedHandler(JobInitializator sender, AJob job);
        public delegate void JobInitializationFailedHandler(JobInitializator sender, AJob job, Exception ex);
        public event JobInitializedHandler JobInitialized;
        public event JobInitializationFailedHandler JobInitializationFailed;

        ConcurrentQueue<AJob> initQueue = new ConcurrentQueue<AJob>();
        Thread initThread;
        bool running = false;

        public JobInitializator()
        {
            initThread = new Thread(Run);
        }

        public void Start()
        {
            lock(initThread)
            {
                if(!running)
                {
                    initThread.Start();
                    running = true;
                }
            }
        }

        public void Stop()
        {
            lock(initThread)
            {
                if(running)
                {
                    initThread.Interrupt();
                    running = false;
                }
            }
        }

        void Run()
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
                            Console.WriteLine("initing job " + job.ID);
                            job.Initialize();
                            JobInitialized?.Invoke(this, job);
                            Console.WriteLine("done");
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
