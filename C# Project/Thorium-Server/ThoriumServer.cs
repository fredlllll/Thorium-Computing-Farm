using Thorium_Shared.Net;
using Newtonsoft.Json.Linq;

namespace Thorium_Server
{
    public class ThoriumServer
    {
        JSONObjectServer jsonObjectServer;
        JSONObjectServer jsonObjectServerClients;
        ClientManager clientManager;

        public ThoriumServer()
        {
            jsonObjectServer = new JSONObjectServer(ThoriumServerConfig.ListeningPort);
            jsonObjectServer.ObjectReceived += JsonObjectServer_ObjectReceived;
            jsonObjectServerClients = new JSONObjectServer(ThoriumServerConfig.ClientListeningPort);
            jsonObjectServerClients.ObjectReceived += JsonObjectServerClients_ObjectReceived;

            clientManager = new ClientManager();
        }

        private void JsonObjectServerClients_ObjectReceived(JObject obj)
        {
            string cmd = obj.Get<string>("command", "");
            switch(cmd)
            {
                //TODO: stuff
                case "getTask":
                    break;
            }
        }

        private void JsonObjectServer_ObjectReceived(JObject obj)
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

        /*public ClientManager ClientManager { get; }
        public JobManager JobManager { get; }

        public ServerWCFInterface ServerWCFInterface { get; }

        public ThoriumServer()
        {
            ClientManager = new ClientManager();
            JobManager = new JobManager();

            ServerWCFInterface = new ServerWCFInterface();

            SharedData.Set(ServerConfigConstants.SharedDataID_ServerConfig, ServerConfig);

            DependencyInjection.Kernel.Bind<IThoriumServerInterfaceForClient>().ToConstant(new ThoriumServerInterfaceForClient(this));
        }*/

        public void Start()
        {
            //TODO: load jobs
            jsonObjectServer.Start();
            clientManager.Start();
        }

        public void Stop()
        {
            jsonObjectServer.Stop();
            clientManager.Stop();
            //TODO: save jobs
        }
    }
}
