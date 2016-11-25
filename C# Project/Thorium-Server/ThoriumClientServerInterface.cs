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
            SharedData.Set(ClientConfigConstants.SharedDataID_ServerInterfaceForClient, this);
        }

        public void RegisterClient(IThoriumClientInterfaceForServer client)
        {
            server.ClientManager.RegisterClient(client);
        }

        public void UnregisterClient(IThoriumClientInterfaceForServer client)
        {
            server.ClientManager.UnregisterClient(client);
        }

        public ITask GetTask(IThoriumClientInterfaceForServer client)
        {
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

        public IServerService GetService(Type type)
        {
            return server.ServerServiceManager.GetService(type);
        }
    }
}
