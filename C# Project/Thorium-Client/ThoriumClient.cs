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
using Thorium_Shared.WCF;

namespace Thorium_Client
{
    public class ThoriumClient
    {
        const string configFileName = "clientconfig.xml";

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
            DependencyInjection.Kernel.Bind<IThoriumClientInterfaceForServer>().ToConstant(clientInterface = new ThoriumClientInterfaceForServer());

            IThoriumServerInterfaceForClient serverInterface;
            DependencyInjection.Kernel.Bind<IThoriumServerInterfaceForClient>().ToConstant(serverInterface = WCFServiceManager.Instance.GetServiceInstance<IThoriumServerInterfaceForClient>(Constants.THORIUM_SERVER_INTERFACE_FOR_CLIENT, clientInterface));

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

        void Run()
        {
            DateTime lastTimeJobCompleted = DateTime.UtcNow;
            var serverInterface = DependencyInjection.Kernel.Get<IThoriumServerInterfaceForClient>();
            var clientInterface = DependencyInjection.Kernel.Get<IThoriumClientInterfaceForServer>();
            try
            {
                while(true)
                {
                    var taskInformation = serverInterface?.GetFreeTaskInformation();
                    if(taskInformation != null)
                    {
                        ATask task = ATask.TaskFromInformation(taskInformation);
                        try
                        {
                            task.Run();
                            serverInterface?.SignalTaskFinished(taskInformation.JobID, taskInformation.ID);
                        }
                        catch(Exception execEx)
                        {
                            serverInterface?.SignalTaskAborted(taskInformation.JobID, taskInformation.ID, execEx.ToString());
                        }
                        lastTimeJobCompleted = DateTime.UtcNow;
                    }
                    else
                    {
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
                Console.WriteLine("thread interrupted");
            }
            catch(Exception ex)
            {
                //TODO: log
                Console.WriteLine("exception: " + ex);
            }
            Shutdown();
        }
    }
}
