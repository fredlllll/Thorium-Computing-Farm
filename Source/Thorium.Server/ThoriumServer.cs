using Thorium.Config;
using Thorium.Server.Data;
using Thorium.Threading;

namespace Thorium.Server
{
    public class ThoriumServer : RestartableThreadClass
    {
        /// <summary>
        /// for control commands
        /// </summary>
        ServerController serverController;
        /// <summary>
        /// for comms between server and clients
        /// </summary>
        ClientsServicePoint clientsServer;

        public TaskManager TaskManager { get; private set; }
        public ClientManager ClientManager { get; private set; }
        public ClientTaskRelationManager ClientTaskRelationManager { get; private set; }

        public DataManager DataManager { get; private set; }


        public ThoriumServer() : base(false)
        {
            DataManager = new DataManager();

            TaskManager = new TaskManager(DataManager.TaskSerializer);
            ClientManager = new ClientManager();
            ClientTaskRelationManager = new ClientTaskRelationManager(DataManager.ClientTaskRelationSerializer);

            //TODO remove ThoriumServerConfig.ListeningPort from config
            serverController = new ServerController(this);
            dynamic config = ConfigFile.GetClassConfig();
            clientsServer = new ClientsServicePoint(this, config.ClientListeningPort);
        }


        public override void Start()
        {
            ClientManager.Start();
            ClientTaskRelationManager.Start();
            serverController.Start();
            clientsServer.Start();
            base.Start();
        }

        public override void Stop(int joinTimeoutms = -1)
        {
            ClientManager.Stop(joinTimeoutms);
            ClientTaskRelationManager.Stop();
            serverController.Stop();
            clientsServer.Stop();
            base.Stop(joinTimeoutms);
        }

        protected override void Run()
        {
            //check database for jobs that can be assigned
            var lt = TaskManager.GetAssignableTask().ToLightweightTask();
            var client = ClientManager.GetFreeClient();
            if(client.AssignTask(lt))
            {
                var rel = new ClientTaskRelation(client.Id, lt.Id);
                ClientTaskRelationManager.Add(rel);
            }
        }
    }
}
