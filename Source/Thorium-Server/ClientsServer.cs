using Newtonsoft.Json.Linq;
using Thorium_Shared;
using Thorium_Shared.Net.Comms;
using static Thorium_Shared.Net.ClientToServerCommands;

namespace Thorium_Server
{
    public class ClientsServer
    {
        ThoriumServer server;

        ServiceServer serviceServer;

        public ClientsServer(ThoriumServer thoriumServer, int port)
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
            switch(command)
            {
                case Register:
                    {
                        Client client = new Client(sender.Remote, argObject.Get<string>("id"));
                        server.ClientManager.RegisterClient(client);
                    }
                    break;
                case Unregister:
                    {
                        server.ClientManager.UnregisterClient(sender.Remote);
                    }
                    break;
                case CheckoutTask:
                    {
                        Task t = server.TaskManager.CheckoutTask();
                        if(t != null)
                        {
                            LightweightTask lt = new LightweightTask(t);
                            JObject retval = JObject.FromObject(lt);
                            return retval;
                        }
                    }
                    break;
                case TurnInTask:
                    {
                        server.TaskManager.TurnInTask(argObject.Get<string>("id"));
                    }
                    break;
                case AbandonTask:
                    {
                        server.TaskManager.AbandonTask(argObject.Get<string>("id"));
                    }
                    break;
            }
            return null;
        }
    }
}
