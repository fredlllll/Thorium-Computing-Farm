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

        private JToken ServiceServer_InvokationReceived(IMessageTransceiver sender, string command, JToken arg)
        {
            JObject argObject = arg as JObject;

            //TODO: parse
            switch(command)
            {
                //TODO: stuff
                case AddJob:
                    {
                        Job j = new Job(Utils.GetRandomID(), argObject.Get<string>("jobName"), (JObject)argObject["jobInformation"]);
                        logger.Info("new Job Added: " + j.ID + ", " + j.Name + ", " + j.Information);
                        server.JobManager.AddJob(j);
                        JObject retval = new JObject
                        {
                            ["id"] = j.ID
                        };
                        return retval;
                    }
                case ListClients:
                    {
                        JArray retval = new JArray();
                        foreach(var c in server.ClientManager.ClientsSnapshot)
                        {
                            retval.Add(c.ID);
                        }
                        return retval;
                    }
                case ListJobs:
                    {
                        JArray retval = new JArray();
                        foreach(var j in server.JobManager.Jobs)
                        {
                            retval.Add(j.Key);
                        }
                        return retval;
                    }
                case ListTasks:
                    {
                        JArray retval = new JArray();
                        foreach(var t in server.TaskManager.Tasks)
                        {
                            retval.Add(t.ID);
                        }
                        return retval;
                    }
                case AbortJob:
                    {
                        string id = argObject.Get<string>("id");
                        foreach(var t in server.TaskManager.Tasks)
                        {
                            if(t.Job.ID == id)
                            {
                                server.TaskManager.AbortTask(t.ID);
                            }
                        }
                    }
                    break;
                case AbortTask:
                    {
                        string id = argObject.Get<string>("id");
                        server.TaskManager.AbortTask(id);
                    }
                    break;
            }
            return null;
        }
    }
}
