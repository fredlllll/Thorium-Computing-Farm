using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ninject;
using Thorium_Shared;
using Codolith.Config;

namespace Thorium_Client
{
    public class ThoriumClient
    {
        const string configFileName = "clientconfig.xml";

        public string ID { get; } = Utils.GetRandomID();

        public ATask CurrentTask { get; private set; }

        Config Config { get; set; }
        Thread runner;
        public ThoriumClient()
        {
            Config = new Config(configFileName);
            SharedData.Set(ClientConfigConstants.SharedDataID_ClientConfig, Config);

            string serverAddress = Config.Get(ClientConfigConstants.ConfigID_WCFAddress);
            int port = Config.Get<int>(ClientConfigConstants.ConfigID_WCFPort);
            WCFServiceManager.Instance.Port = port;
            WCFServiceManager.Instance.RemoteHost = serverAddress;

            IThoriumClientInterfaceForServer clientInterface;
            clientInterface = new ThoriumClientInterfaceForServer(this);
            DependencyInjection.Kernel.Bind<IThoriumClientInterfaceForServer>().ToConstant(clientInterface);

            IThoriumServerInterfaceForClient serverInterface;
            serverInterface = WCFServiceManager.Instance.GetServiceInstance<IThoriumServerInterfaceForClient>(Constants.THORIUM_SERVER_INTERFACE_FOR_CLIENT, clientInterface);
            DependencyInjection.Kernel.Bind<IThoriumServerInterfaceForClient>().ToConstant(serverInterface);
            Thread.Sleep(1000);

            serverInterface.RegisterClient();//i basically have to tell the server how he can reach me... but i dunno how. ip could be local or external, but what about stupid routers without loopback?
            runner = new Thread(Run);

            SharedData.Set(ClientConfigConstants.SharedDataID_ThoriumClient, this);
        }

        public void Start()
        {
            runner.Start();
        }

        public void Shutdown()
        {
            var serverInterface = DependencyInjection.Kernel.Get<IThoriumServerInterfaceForClient>();
            var clientInterface = DependencyInjection.Kernel.Get<IThoriumClientInterfaceForServer>();
            serverInterface.UnregisterClient();
            serverInterface = null;
            runner.Interrupt();

            WCFServiceManager.Instance.FreeServiceInstance<IThoriumServerInterfaceForClient>();
        }

        public void AbortTask(string id)
        {
            if(CurrentTask.GetID() == id)
            {
                CurrentTask.Abort();
                var serverInterface = DependencyInjection.Kernel.Get<IThoriumServerInterfaceForClient>();
                serverInterface.SignalTaskAborted(CurrentTask.GetJobID(), id, "Aborted");
            }
        }

        void Run()
        {
            DateTime lastTimeJobCompleted = DateTime.UtcNow;
            var serverInterface = DependencyInjection.Kernel.Get<IThoriumServerInterfaceForClient>();
            var clientInterface = DependencyInjection.Kernel.Get<IThoriumClientInterfaceForServer>();
            try
            {
                while(true)
                {
                    Logger.Log("getting job...");
                    var taskInformation = serverInterface?.GetFreeTaskInformation();
                    if(taskInformation != null)
                    {
                        Logger.Log("got task: " + taskInformation.ID);
                        CurrentTask = ATask.TaskFromInformation(taskInformation);
                        try
                        {
                            Logger.Log("executing task");
                            CurrentTask.Run();
                            serverInterface?.SignalTaskFinished(taskInformation.JobID, taskInformation.ID);
                            Logger.Log("done task");
                        }
                        catch(Exception execEx) when (!(execEx is ThreadInterruptedException))
                        {
                            Logger.Log("task failed: " +execEx);
                            serverInterface?.SignalTaskAborted(taskInformation.JobID, taskInformation.ID, execEx.ToString());
                        }
                        CurrentTask = default(ATask);
                        lastTimeJobCompleted = DateTime.UtcNow;
                    }
                    else
                    {
                        Logger.Log("no task available");
                        //TODO: make shutdown an option
                        if((DateTime.UtcNow - lastTimeJobCompleted).TotalSeconds > 180) //if idle for x seconds we shutdown
                        {
                            break;
                        }
                        Thread.Sleep(5000);
                    }
                }
            }
            catch(ThreadInterruptedException)
            {
                //bye bye
                Logger.Log("worker thread interrupted. exiting");
            }
            catch(Exception ex)
            {
                //TODO: log
                Logger.Log("exception: " + ex);
            }
            Shutdown();
        }
    }
}
