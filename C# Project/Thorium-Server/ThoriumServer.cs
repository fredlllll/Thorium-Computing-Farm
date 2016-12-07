using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;
using Ninject;

namespace Thorium_Server
{
    public class ThoriumServer
    {
        const string configFileName = "serverconfig.xml";

        public Config Config { get; private set; }
        //ServiceHost serviceHost;
        //ThoriumClientServerInterface serverInterface;

        public ClientManager ClientManager { get; } = new ClientManager();
        public TaskManager TaskManager { get; } = new TaskManager();
        public JobManager JobManager { get; }
        //public ServerServiceManager ServerServiceManager { get; } = new ServerServiceManager();

        public ThoriumServer()
        {
            Config = new Config(new FileInfo(configFileName));
            JobManager = new JobManager(TaskManager, this);
            TaskManager.JobManager = JobManager;
            TaskManager.ClientManager = ClientManager;

            int wcfPort = Config.GetInt(ServerConfigConstants.wcfPort);
            WCFServiceManager.Instance.Port = wcfPort;

            SharedData.Set(ServerConfigConstants.SharedDataID_ServerConfig, this.Config);

            DependencyInjection.Kernel.Bind<IThoriumServerInterfaceForClient>().ToConstant(new ThoriumClientServerInterface(this));
        }

        public void Start()
        {
            WCFServiceManager.Instance.HostServiceInstance(DependencyInjection.Kernel.Get<IThoriumServerInterfaceForClient>(), Constants.THORIUM_SERVER_INTERFACE_FOR_CLIENT);
            /*NetTcpBinding tcpBinding = new NetTcpBinding();
            Uri wcfAddress = new Uri("net.tcp://localhost:" + instanceServerPort + "/" + Constants.THORIUM_SERVER_INTERFACE_FOR_CLIENT);
            serviceHost = new ServiceHost(serverInterface, wcfAddress);
            serviceHost.AddServiceEndpoint(typeof(IThoriumServerInterfaceForClient), tcpBinding, wcfAddress);
            serviceHost.Open();*/

            JobManager.Initialize();
        }

        public void Stop()
        {
            //serviceHost.Close();
            WCFServiceManager.Instance.UnhostServiceInstance(DependencyInjection.Kernel.Get<IThoriumServerInterfaceForClient>());
            //TODO: save stuff
        }
    }
}
