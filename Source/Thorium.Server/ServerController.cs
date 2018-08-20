using Newtonsoft.Json.Linq;
using NLog;
using Thorium.Net.ServiceHost;
using Thorium.Net.ServiceHost.InvokationHandlers;
using Thorium.Shared;
using static Thorium.Shared.Net.ServerControlCommands;

namespace Thorium.Server
{
    public class ServerController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ThoriumServer server;

        private ServiceHost serviceHost;

        public ServerController(ThoriumServer server)
        {
            this.server = server;

            serviceHost = new ServiceHost("server_controller");

            RoutineInvokationHandler rih = new RoutineInvokationHandler();

            rih.RegisterRoutine(new Routine(AddTask, AddTaskHandler));
            rih.RegisterRoutine(new Routine(GetTaskStatus, GetTaskStatusHandler));
            rih.RegisterRoutine(new Routine(QueueTask, QueueTaskHandler));
            rih.RegisterRoutine(new Routine(AbortTask, AbortTaskHandler));

            serviceHost.RegisterInvokationHandler(rih);
        }

        public void Start()
        {
            serviceHost.Start();
        }

        public void Stop()
        {
            serviceHost.Stop();
        }

        JToken AddTaskHandler(JToken arg)
        {
            JObject argObject = (JObject)arg;

            string id = Utils.Utils.GetRandomGUID();
            JObject information = argObject.Get<JObject>("information");
            server.TaskManager.AddTask(id, information, TaskStatus.Paused);
            logger.Info("New Task added: " + id + ", " + information);

            return id;
        }

        JToken GetTaskStatusHandler(JToken arg)
        {
            string id = arg.Value<string>();
            TaskStatus status = server.TaskManager.GetTaskStatus(id);
            return status.ToString();
        }

        JToken QueueTaskHandler(JToken arg)
        {
            string id = arg.Value<string>();
            return server.TaskManager.QueueTask(id);
        }

        JToken AbortTaskHandler(JToken arg)
        {
            string id = arg.Value<string>();
            //TODO: abort on client too
            return server.TaskManager.AbortTask(id);
        }
    }
}
