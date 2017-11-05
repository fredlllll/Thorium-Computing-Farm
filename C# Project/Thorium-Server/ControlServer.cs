using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Thorium_Shared.Net;
using Thorium_Shared.Net.Comms;
using static Thorium_Shared.Net.ServerControlCommands;

namespace Thorium_Server
{
    public class ControlServer
    {
        ThoriumServer server;

        ServiceServer serviceServer;

        public ControlServer(ThoriumServer thoriumServer, int port)
        {
            server = thoriumServer;
            serviceServer = CommsFactory.CreateServer((ushort)port);
            serviceServer.InvokationReceived += ServiceServer_InvokationReceived;
        }

        public void Start()
        {
            serviceServer.Start();
        }

        public void Stop()
        {
            serviceServer.Stop();
        }

        private JObject ServiceServer_InvokationReceived(IMessageTransceiver sender, string command, JObject arg)
        {
            //TODO: parse
            switch(command)
            {
                //TODO: stuff
                case AddJob:
                    //TODO: create job from info and add tasks to database
                    break;
            }
            return null;
        }
    }
}
