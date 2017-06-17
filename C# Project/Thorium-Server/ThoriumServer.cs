using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;
using Codolith.Config;
using Ninject;
using static Thorium_Server.ServerStatics;
using Codolith.Logging;
using Thorium_Shared.WCFInterfaces;

namespace Thorium_Server
{
    public class ThoriumServer
    {
        public ClientManager ClientManager { get; }
        public JobManager JobManager { get; }

        public ServerWCFInterface ServerWCFInterface { get; }

        public ThoriumServer()
        {
            ClientManager = new ClientManager();
            JobManager = new JobManager();

            ServerWCFInterface = new ServerWCFInterface();

            SharedData.Set(ServerConfigConstants.SharedDataID_ServerConfig, ServerConfig);

            DependencyInjection.Kernel.Bind<IThoriumServerInterfaceForClient>().ToConstant(new ThoriumServerInterfaceForClient(this));
        }

        public void Start()
        {
            ServerWCFInterface.Start();
            JobManager.Start(); //Load jobs
        }

        public void Stop()
        {
            ServerWCFInterface.Stop();
            JobManager.Shutdown(); //Save jobs
        }
    }
}
