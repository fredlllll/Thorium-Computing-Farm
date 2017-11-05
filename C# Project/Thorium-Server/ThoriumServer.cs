using Thorium_Shared.Net;
using Newtonsoft.Json.Linq;

namespace Thorium_Server
{
    public class ThoriumServer
    {
        /// <summary>
        /// for control commands
        /// </summary>
        ControlServer controlServer;
        /// <summary>
        /// for comms between server and clients
        /// </summary>
        ClientsServer clientsServer;

        public JobManager JobManager { get; private set; }
        public ClientManager ClientManager { get; private set; }
        public TaskManager TaskManager { get; private set; }


        public ThoriumServer()
        {
            JobManager = new JobManager();
            TaskManager = new TaskManager();
            ClientManager = new ClientManager();

            controlServer = new ControlServer(this, ThoriumServerConfig.ListeningPort);
            clientsServer = new ClientsServer(this, ThoriumServerConfig.ClientListeningPort);
        }


        public void Start()
        {
            //TODO: load jobs
            controlServer.Start();
            clientsServer.Start();
            ClientManager.Start();
        }

        public void Stop()
        {
            controlServer.Stop();
            clientsServer.Stop();
            ClientManager.Stop();
            //TODO: save jobs
        }
    }
}
