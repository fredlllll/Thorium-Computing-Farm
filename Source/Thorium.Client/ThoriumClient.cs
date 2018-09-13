using System;
using System.Threading;
using Thorium.Shared;
using NLog;
using Thorium.Config;
using Thorium.Threading;

namespace Thorium.Client
{
    public class ThoriumClient : RestartableThreadClass
    {
        private static dynamic config = ConfigFile.GetClassConfig();

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ServerInterface serverInterface;
        private ClientController clientController;

        //TODO: make id created by a pattern, so single machines can be identified
        public string Id { get; } = Utils.Utils.GetRandomGUID();

        private AutoResetEvent hasTaskEvent = new AutoResetEvent(false);
        public LightweightTask CurrentTask { get; private set; } = null;

        private NewTaskRequester taskRequester;

        public ThoriumClient() : base(false)
        {
            string host = config.ServerHost;
            ushort port = config.ServerListeningPort;

            serverInterface = new ServerInterface(host, port, this);
            clientController = new ClientController(this);

            taskRequester = new NewTaskRequester(this, serverInterface);
        }

        public override void Start()
        {
            serverInterface.InvokeRegister();
            clientController.Start();
            taskRequester.Start();
            base.Start();
        }

        public override void Stop(int joinTimeoutms = -1)
        {
            string id = CurrentTask?.Id;
            if(id != null)
            {
                serverInterface.InvokeAbandonTask(id, "Client Stopped");
            }
            serverInterface.InvokeUnregister();
            clientController.Stop();
            taskRequester.Stop();

            base.Stop(joinTimeoutms);
        }

        public bool AssignTask(LightweightTask lightweightTask)
        {
            if(CurrentTask == null)
            {
                CurrentTask = lightweightTask;
                hasTaskEvent.Set();
                return true;
            }
            return false;
        }

        protected override void Run()
        {
            DateTime lastTimeJobCompleted = DateTime.UtcNow;
            try
            {
                while(true)
                {
                    logger.Info("getting job...");
                    hasTaskEvent.WaitOne();
                    logger.Info("got task: " + CurrentTask.Id);

                    AExecutioner executioner = CurrentTask.GetExecutioner();
                    try
                    {
                        logger.Info("executing task");
                        var result = executioner.Execute();
                        switch(result.FinalAction)
                        {
                            case FinalAction.TurnIn:
                                serverInterface.InvokeTurnInTask(CurrentTask.Id, result.AdditionalInformation);
                                break;
                            case FinalAction.Abandon:
                                serverInterface.InvokeAbandonTask(CurrentTask.Id, result.AdditionalInformation);
                                break;
                            case FinalAction.Fail:
                                serverInterface.InvokeFailTask(CurrentTask.Id, result.AdditionalInformation);
                                break;
                        }

                        logger.Info("done task");
                    }
                    catch(Exception execEx) when(!(execEx is ThreadInterruptedException))
                    {
                        logger.Info("task failed: " + execEx);
                        serverInterface.InvokeFailTask(CurrentTask.Id, execEx.ToString());
                    }
                    CurrentTask = null;
                    lastTimeJobCompleted = DateTime.UtcNow;
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
