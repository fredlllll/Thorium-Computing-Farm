using System.Net;
using Newtonsoft.Json.Linq;
using Thorium_Shared;
using Thorium_Shared.Net.ServicePoint;
using static Thorium_Shared.Net.ClientToServerCommands;

namespace Thorium_Server
{
    public class ClientsServicePoint
    {
        ThoriumServer server;

        Thorium_Shared.Net.ServicePoint.ServicePoint servicePoint = new Thorium_Shared.Net.ServicePoint.ServicePoint();

        public ClientsServicePoint(ThoriumServer thoriumServer, int port)
        {
            server = thoriumServer;

            servicePoint.RegisterInvokationReceiver(new TCPServiceInvokationReceiver(port));

            servicePoint.RegisterRoutine(new Routine(Register, HandleRegister));
            servicePoint.RegisterRoutine(new Routine(Unregister, HandleUnregister));
            servicePoint.RegisterRoutine(new Routine(CheckoutTask, HandleCheckoutTask));
            servicePoint.RegisterRoutine(new Routine(TurnInTask, HandleTurnInTask));
            servicePoint.RegisterRoutine(new Routine(AbandonTask, HandleAbandonTask));
        }

        public void Start()
        {
            servicePoint.Start();
        }

        public void Stop()
        {
            servicePoint.Stop();
        }

        JToken HandleRegister(JToken arg)
        {
            JObject argObject = arg as JObject;

            Client client = new Client(IPAddress.Parse(argObject.Get<string>("ip")), argObject.Get<string>("id"));
            server.ClientManager.RegisterClient(client);

            return null;
        }

        JToken HandleUnregister(JToken arg)
        {
            JObject argObject = arg as JObject;

            server.ClientManager.UnregisterClient(argObject.Get<string>("id"));

            return null;
        }

        JToken HandleCheckoutTask(JToken arg)
        {
            JObject argObject = arg as JObject;

            Task t = server.TaskManager.CheckoutTask();
            if(t != null)
            {
                //TODO: keep track of what client processes what task
                LightweightTask lt = new LightweightTask(t);
                JObject retval = JObject.FromObject(lt);
                return retval;
            }
            return null;
        }

        JToken HandleTurnInTask(JToken arg)
        {
            JObject argObject = arg as JObject;

            server.TaskManager.TurnInTask(argObject.Get<string>("id"));

            return null;
        }

        JToken HandleAbandonTask(JToken arg)
        {
            JObject argObject = arg as JObject;

            server.TaskManager.AbandonTask(argObject.Get<string>("id"));

            return null;
        }
    }
}
