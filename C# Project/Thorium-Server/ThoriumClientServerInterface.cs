using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using Thorium_Shared;
using Thorium_Shared.Services;

namespace Thorium_Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ThoriumClientServerInterface : IThoriumServerInterfaceForClient
    {
        ThoriumServer server;

        public ThoriumClientServerInterface(ThoriumServer server)
        {
            this.server = server;
        }

        public void RegisterClient(string clientID)
        {
            //string host = Util.GetWCFClientHost();
            var client = OperationContext.Current.GetCallbackChannel<IThoriumClientInterfaceForServer>();
            //var client = WCFServiceManager.Instance.GetServiceInstance<IThoriumClientInterfaceForServer>(Constants.THORIUM_CLIENT_INTERFACE_FOR_SERVER, host);
            server.ClientManager.RegisterClient(client);
        }

        public void UnregisterClient(string clientID)
        {
            server.ClientManager.UnregisterClient(clientID);
        }

        public ITask GetTask(string clientID)
        {
            return server.TaskManager.GetTask(clientID);
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
