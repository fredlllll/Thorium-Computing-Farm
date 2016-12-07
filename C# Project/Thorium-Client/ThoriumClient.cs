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

namespace Thorium_Client
{
    public class ThoriumClient
    {
        const string configFileName = "clientconfig.xml";

        Config Config { get; set; }
        Thread runner;
        public ThoriumClient()
        {
            Config = new Config(new FileInfo(configFileName));
            SharedData.Set(ClientConfigConstants.SharedDataID_ClientConfig, Config);

            string serverAddress = Config.GetString(ClientConfigConstants.ConfigID_WCFAddress);
            int port = Config.GetInt(ClientConfigConstants.ConfigID_WCFPort);
            WCFServiceManager.Instance.Port = port;
            WCFServiceManager.Instance.RemoteHost = serverAddress;

            IThoriumServerInterfaceForClient serverInterface;
            DependencyInjection.Kernel.Bind<IThoriumServerInterfaceForClient>().ToConstant(serverInterface = WCFServiceManager.Instance.GetServiceInstance<IThoriumServerInterfaceForClient>(Constants.THORIUM_SERVER_INTERFACE_FOR_CLIENT));
            IThoriumClientInterfaceForServer clientInterface;
            DependencyInjection.Kernel.Bind<IThoriumClientInterfaceForServer>().ToConstant(clientInterface = new ThoriumClientInterfaceForServer());
            WCFServiceManager.Instance.HostServiceInstance(clientInterface, Constants.THORIUM_CLIENT_INTERFACE_FOR_SERVER);

            serverInterface.RegisterClient(clientInterface.GetID());//i basically have to tell the server how he can reach me... but i dunno how. ip could be local or external, but what about stupid routers without loopback?
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
            serverInterface.UnregisterClient(clientInterface.GetID());
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
                    var task = serverInterface?.GetTask(clientInterface.GetID());
                    if(task != null)
                    {
                        var execInfo = task.GetExecutionInfo();
                        try
                        {
                            execInfo.Setup();
                            execInfo.Run();
                            execInfo.Cleanup();
                            serverInterface?.TurnInTask(task);//this can be put in a seperate thread at some point
                        }
                        catch(Exception execEx)
                        {
                            serverInterface?.ReturnUnfinishedTask(task, execEx.ToString());
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
            }
            catch(Exception ex)
            {
                //TODO: log
                serverInterface?.UnregisterClient(clientInterface.GetID());
                Util.ShutdownSystem();
            }

        }
    }
}
