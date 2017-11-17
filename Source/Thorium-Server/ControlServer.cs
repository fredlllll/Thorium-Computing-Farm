using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NLog;
using Thorium_Shared;
using Thorium_Shared.Net;
using Thorium_Shared.Net.Comms;
using static Thorium_Shared.Net.ServerControlCommands;

namespace Thorium_Server
{
    public class ControlServer
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

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
                    Job j = new Job(Utils.GetRandomID(), arg.Get<string>("jobName"), (JObject)arg["jobInformation"]);
                    logger.Info("new Job Added: " + j.ID + ", " + j.Name + ", " + j.Information);
                    server.JobManager.AddJob(j);
                    JObject retval = new JObject
                    {
                        ["id"] = j.ID
                    };
                    return retval;
            }
            return null;
        }
    }
}
