using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Thorium.Net.ServiceHost;
using Thorium.Net.ServiceHost.InvokationHandlers;
using Thorium.Shared;
using static Thorium.Shared.Net.ClientControlCommands;

namespace Thorium.Client
{
    public class ClientController
    {
        private readonly ThoriumClient client;

        private ServiceHost serviceHost;

        public ClientController(ThoriumClient client)
        {
            this.client = client;

            serviceHost = new ServiceHost("client_controller");

            RoutineInvokationHandler rih = new RoutineInvokationHandler();

            rih.RegisterRoutine(new Routine(AssignTask, AssignTaskHandler));

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

        JToken AssignTaskHandler(JToken arg)
        {
            if(arg is JObject argObject)
            {
                client.AssignTask(argObject.ToObject<LightweightTask>());
                return true;
            }
            return false;
        }
    }
}
