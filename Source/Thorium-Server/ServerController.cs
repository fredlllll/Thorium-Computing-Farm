using Newtonsoft.Json.Linq;
using NLog;
using Thorium_Shared;
using Thorium_Shared.Config;
using Thorium_Shared.Net.ServicePoint;
using static Thorium_Shared.Net.ServerControlCommands;

namespace Thorium_Server
{
    public class ServerController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ThoriumServer server;

        private ServicePoint servicePoint;

        public ServerController(ThoriumServer server)
        {
            this.server = server;

            servicePoint = new ServicePoint("server_controller_service_point");

            //servicePoint.RegisterInvokationReceiver(new HttpServiceInvokationReceiver());
            //var config = ConfigFile.GetConfig("thorium_server");
            //servicePoint.RegisterInvokationReceiver(new TCPServiceInvokationReceiver(config.ListeningPort));

            servicePoint.RegisterRoutine(new Routine(AddJob, AddJobHandler));
            servicePoint.RegisterRoutine(new Routine(AbortJob, AbortJobHandler));
            servicePoint.RegisterRoutine(new Routine(AbortTask, AbortTaskHandler));
        }

        public void Start()
        {
            servicePoint.Start();
        }

        public void Stop()
        {
            servicePoint.Stop();
        }

        JToken AddJobHandler(JToken arg)
        {
            JObject argObject = (JObject)arg;

            Job j = new Job(Utils.GetRandomID(), argObject.Get<string>("jobName"), (JObject)argObject["jobInformation"], JobStatus.Initializing);
            logger.Info("new Job Added: " + j.ID + ", " + j.Name + ", " + j.Information);
            server.JobManager.AddJob(j);
            JObject retval = new JObject
            {
                ["id"] = j.ID
            };
            return retval;
        }

        JToken AbortJobHandler(JToken arg)
        {
            JObject argObject = (JObject)arg;

            string id = argObject.Get<string>("id");
            foreach(var t in server.DataManager.TaskSerializer.LoadWhere("job_id", id))
            {
                server.TaskManager.AbortTask(t.ID);
            }
            return null;
        }

        JToken AbortTaskHandler(JToken arg)
        {
            JObject argObject = (JObject)arg;

            string id = argObject.Get<string>("id");
            server.TaskManager.AbortTask(id);

            return null;
        }
    }
}
