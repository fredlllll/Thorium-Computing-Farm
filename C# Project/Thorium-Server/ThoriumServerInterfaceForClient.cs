using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using Thorium_Shared;
using Thorium_Shared.Services;
using Thorium_Shared.WCF;

namespace Thorium_Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true,ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ThoriumServerInterfaceForClient : IThoriumServerInterfaceForClient
    {
        ThoriumServer server;

        public ThoriumServerInterfaceForClient(ThoriumServer server)
        {
            this.server = server;
        }

        public void RegisterClient()
        {
            var client = OperationContext.Current.GetCallbackChannel<IThoriumClientInterfaceForServer>();
            server.ClientManager.RegisterClient(client);
        }

        public void UnregisterClient()
        {
            var client = OperationContext.Current.GetCallbackChannel<IThoriumClientInterfaceForServer>();
            server.ClientManager.UnregisterClient(client);
        }

        public ITask GetTask()
        {
            var client = OperationContext.Current.GetCallbackChannel<IThoriumClientInterfaceForServer>();
            return server.TaskManager.GetTask(client);
        }

        public void ReturnUnfinishedTask(ITask task, string reason)
        {
            Console.WriteLine("unfinished Task returned: " + reason);
            server.TaskManager.ReturnUnfinishedTask(task);
        }

        public void TurnInTask(ITask task)
        {
            server.TaskManager.TurnInTask(task);
        }

        public string GetServicePath(Type interfaceType)
        {
            var service = ServiceManager.Instance.GetService(interfaceType);
            var address = WCFServiceManager.Instance.HostServiceInstance(service);
            return address;
        }
    }
}
