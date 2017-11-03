using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Thorium_Shared;
using Thorium_Shared.Net;

namespace Thorium_Server
{
    public class ClientsServer : JSONObjectServer
    {
        ThoriumServer server;

        public ClientsServer(ThoriumServer thoriumServer, int port) : base(port)
        {
            ObjectReceived += ClientsServer_ObjectReceived;
            server = thoriumServer;
        }

        private void ClientsServer_ObjectReceived(JObject obj)
        {
            string cmd = obj.Get<string>("command", "");
            switch(cmd)
            {
                case ClientToServerCommands.GetTask:
                    break;
                case ClientToServerCommands.FinishTask:
                    break;
                case ClientToServerCommands.AbandonTask:
                    break;
            }
        }
    }
}
