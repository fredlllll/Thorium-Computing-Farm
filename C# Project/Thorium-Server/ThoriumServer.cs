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

        public ClientManager ClientManager { get; } = new ClientManager();
        public TaskManager TaskManager { get; } = new TaskManager();
        public JobManager JobManager { get; }

        public ThoriumServer()
        {
            Config = new Config(new FileInfo(configFileName));
            JobManager = new JobManager(TaskManager, this);
            TaskManager.JobManager = JobManager;
            TaskManager.ClientManager = ClientManager;

            int wcfPort = Config.GetInt(ServerConfigConstants.wcfPort);
            WCFServiceManager.Instance.Port = wcfPort;

            SharedData.Set(ServerConfigConstants.SharedDataID_ServerConfig, this.Config);

            DependencyInjection.Kernel.Bind<IThoriumServerInterfaceForClient>().ToConstant(new ThoriumServerInterfaceForClient(this));
        }

        public void Start()
        {
            WCFServiceManager.Instance.HostServiceInstance(DependencyInjection.Kernel.Get<IThoriumServerInterfaceForClient>(), Constants.THORIUM_SERVER_INTERFACE_FOR_CLIENT);
            JobManager.Initialize();
        }

        public void Stop()
        {
            WCFServiceManager.Instance.UnhostServiceInstance(DependencyInjection.Kernel.Get<IThoriumServerInterfaceForClient>());
            //TODO: save stuff
        }
    }
}
