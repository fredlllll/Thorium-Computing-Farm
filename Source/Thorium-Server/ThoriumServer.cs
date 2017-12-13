using Thorium_Shared.Net;
using Newtonsoft.Json.Linq;

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
        ClientsServer clientsServer;

        public JobManager JobManager { get; private set; }
        public ClientManager ClientManager { get; private set; }
        public TaskManager TaskManager { get; private set; }


        public ThoriumServer()
        {
            JobManager = new JobManager(this);
            TaskManager = new TaskManager();
            ClientManager = new ClientManager();

            //TODO remove ThoriumServerConfig.ListeningPort from config
            serverController = new ServerController(this);
            clientsServer = new ClientsServer(this, ThoriumServerConfig.ClientListeningPort);
        }


        public void Start()
        {
            //TODO: load jobs
            JobManager.Start();
            ClientManager.Start();
            serverController.Start();
            clientsServer.Start();
            ClientManager.Start();
        }

        public void Stop()
        {
            JobManager.Stop();
            ClientManager.Stop();
            serverController.Stop();
            clientsServer.Stop();
            ClientManager.Stop();
            //TODO: save jobs
        }
    }
}
