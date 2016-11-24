using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Thorium_Shared;
using Thorium_Shared.Services;

namespace Thorium_Server
{
    public class ThoriumClientServerInterface : MarshalByRefObject, IThoriumServerInterfaceForClient
    {
        ThoriumServer server;

        public override object InitializeLifetimeService()
        {
            return null; //infinite lease lifetime
        }

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

        public Task GetTask(IThoriumClientInterfaceForServer client)
        {
            return server.TaskManager.GetTask(client);
        }

        public void ReturnUnfinishedTask(Task task, string reason)
        {
            Console.WriteLine("unfinished Task returned: " + reason);
            server.TaskManager.ReturnUnfinishedTask(task);
        }

        public void TurnInTask(Task task)
        {
            server.TaskManager.TurnInTask(task);
        }

        public AServerService GetService(Type type)
        {
            return server.ServerServiceManager.GetService(type);
        }
    }
}
