using System;
using System.Threading;
using Thorium_Shared;
using NLog;
using Thorium_Config;
using Thorium_Threading;
using Thorium_Utils;

namespace Thorium_Client
{
    public class ThoriumClient : RestartableThreadClass
    {
        private static dynamic config = ConfigFile.GetClassConfig();

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ServerInterface serverInterface;

        public string ID { get; } = Utils.GetRandomGUID();

        public ThoriumClient() : base(false)
        {
            serverInterface = new ServerInterface(config.ServerHost, config.ServerListeningPort, this);
        }

        public override void Start()
        {
            serverInterface.InvokeRegister();
            base.Start();
        }

        public override void Stop()
        {
            serverInterface.InvokeUnregister();
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
                        lightweightTask = serverInterface.InvokeCheckoutTask();
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
                            serverInterface.InvokeTurnInTask(lightweightTask);
                            logger.Info("done task");
                        }
                        catch(Exception execEx) when(!(execEx is ThreadInterruptedException))
                        {
                            logger.Info("task failed: " + execEx);
                            serverInterface.InvokeFailTask(lightweightTask, execEx.ToString());
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
