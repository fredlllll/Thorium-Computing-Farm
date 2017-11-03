using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Thorium_Shared.Net;

namespace Thorium_Server
{
    public class ControlServer : JSONObjectServer
    {
        ThoriumServer server;

        public ControlServer(ThoriumServer thoriumServer, int port) : base(port)
        {
            ObjectReceived += ControlServer_ObjectReceived;
            server = thoriumServer;
        }

        private void ControlServer_ObjectReceived(JObject obj)
        {
            //TODO: parse
            string cmd = obj.Get<string>("command", "");
            switch(cmd)
            {
                //TODO: stuff
                case "addJob":
                    //TODO: create job from info and add tasks to database
                    break;
            }
        }
    }
}
