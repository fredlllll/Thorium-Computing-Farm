using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Server
{
    /*public class ServerWCFInterface
    {
        public ServerWCFInterface()
        {
            int wcfPort = ServerConfig.Get<int>(Key_WcfPort);
            WCFServiceManager.Instance.Port = wcfPort;
        }

        public void Start()
        {
            WCFServiceManager.Instance.HostServiceInstance(DependencyInjection.Kernel.Get<IThoriumServerInterfaceForClient>(), Constants.THORIUM_SERVER_INTERFACE_FOR_CLIENT);
        }

        public void Stop()
        {
            WCFServiceManager.Instance.UnhostServiceInstance(DependencyInjection.Kernel.Get<IThoriumServerInterfaceForClient>());
        }
    }*/
}
