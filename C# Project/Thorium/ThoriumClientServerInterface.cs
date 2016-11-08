using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Thorium_Shared;

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
        }

        public bool RegisterClient(IThoriumClientInterfaceForServer client)
        {
            return server.ClientManager.RegisterClient(client);
        }

        public void UnregisterClient(IThoriumClientInterfaceForServer client)
        {
            server.ClientManager.UnregisterClient(client);
        }

        public Task GetJobPart(IThoriumClientInterfaceForServer client)
        {
            return server.TaskManager.GetTask(client);
        }

        public void FinishJobPart(Task task) {
            server.TaskManager.TurnInTask(task);
        }
    }
}
