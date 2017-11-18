using System;
using System.Threading;
using Thorium_Shared;
using NLog;

namespace Thorium_Client
{
    public class ThoriumClient : RestartableThreadClass
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ServerInterface serverInterface;

        public string ID { get; } = Utils.GetRandomID();

        public ThoriumClient() : base(false)
        {
            serverInterface = new ServerInterface(ThoriumClientConfig.ServerHost, ThoriumClientConfig.ServerListeningPort);
        }

        public override void Start()
        {
            serverInterface.Register();
            base.Start();
        }

        public override void Stop()
        {
            serverInterface.Unregister();
            base.Stop();
        }

        protected override void Run()
        {
            DateTime lastTimeJobCompleted = DateTime.UtcNow;
            try
            {
                while(true)
                {
                    logger.Info("getting job...");
                    LightweightTask lightweightTask = null;
                    try
                    {
                        lightweightTask = serverInterface.CheckoutTask();
                    }
                    catch(TimeoutException)
                    {
                        logger.Info("timeout when checking out task...");
                    }
                    if(lightweightTask != null)
                    {
                        logger.Info("got task: " + lightweightTask.ID);

                        AExecutioner executioner = lightweightTask.GetExecutioner();
                        try
                        {
                            logger.Info("executing task");
                            executioner.Execute();
                            serverInterface.TurnInTask(lightweightTask);
                            logger.Info("done task");
                        }
                        catch(Exception execEx) when(!(execEx is ThreadInterruptedException))
                        {
                            logger.Info("task failed: " + execEx);
                            serverInterface.AbandonTask(lightweightTask);
                        }

                        lastTimeJobCompleted = DateTime.UtcNow;
                    }
                    else
                    {
                        /*if((DateTime.UtcNow - lastTimeJobCompleted).TotalSeconds > 180) //if idle for x seconds we shutdown
                        {
                            break;
                        }*/
                        logger.Info("no task available, waiting 5 seconds");
                        Thread.Sleep(5000);
                    }
                }
            }
            catch(ThreadInterruptedException)
            {
                //bye bye
                logger.Info("worker thread interrupted. exiting");
            }
            catch(Exception ex)
            {
                logger.Info("exception");
                logger.Info(ex);
            }
            logger.Info("leaving worker thread");
            Stop();
        }
    }
}
