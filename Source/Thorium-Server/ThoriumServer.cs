using Thorium_Server.Data;

namespace Thorium_Server
{
    public class ThoriumServer
    {
        /// <summary>
        /// for control commands
        /// </summary>
        ServerController serverController;
        /// <summary>
        /// for comms between server and clients
        /// </summary>
        ClientsServicePoint clientsServer;

        public JobManager JobManager { get; private set; }
        public TaskManager TaskManager { get; private set; }
        public ClientManager ClientManager { get; private set; }
        public ClientTaskRelationManager ClientTaskRelationManager { get; private set; }

        public DataManager DataManager { get; private set; }


        public ThoriumServer()
        {
            DataManager = new DataManager();

            JobManager = new JobManager(this, DataManager.JobSerializer, DataManager.TaskSerializer);
            TaskManager = new TaskManager(DataManager.TaskSerializer);
            ClientManager = new ClientManager();
            ClientTaskRelationManager = new ClientTaskRelationManager(DataManager.ClientTaskRelationSerializer);

            //TODO remove ThoriumServerConfig.ListeningPort from config
            serverController = new ServerController(this);
            clientsServer = new ClientsServicePoint(this, ThoriumServerConfig.ClientListeningPort);
        }


        public void Start()
        {
            //TODO: load jobs
            JobManager.Start();
            ClientManager.Start();
            ClientTaskRelationManager.Start();
            serverController.Start();
            clientsServer.Start();
            ClientManager.Start();
        }

        public void Stop()
        {
            JobManager.Stop();
            ClientManager.Stop();
            ClientTaskRelationManager.Stop();
            serverController.Stop();
            clientsServer.Stop();
            ClientManager.Stop();
            //TODO: save jobs
        }
    }
}
