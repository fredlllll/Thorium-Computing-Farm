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

        ClientManager clientManager;

        public ThoriumServer()
        {
            JobManager = new JobManager();

            controlServer = new ControlServer(this, ThoriumServerConfig.ListeningPort);
            clientsServer = new ClientsServer(this, ThoriumServerConfig.ClientListeningPort);

            clientManager = new ClientManager();
        }


        public void Start()
        {
            //TODO: load jobs
            controlServer.Start();
            clientsServer.Start();
            clientManager.Start();
        }

        public void Stop()
        {
            controlServer.Stop();
            clientsServer.Stop();
            clientManager.Stop();
            //TODO: save jobs
        }
    }
}
