using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Thorium_Shared;
using Thorium_Shared.Net.Comms;
using Thorium_Shared.Net;
using static Thorium_Shared.Net.ClientToServerCommands;

namespace Thorium_Client
{
    public class ServerInterface
    {
        ServiceClient serviceClient;
        public ServerInterface(string host, ushort port)
        {
            serviceClient = CommsFactory.CreateClient(host, port);
        }

        public void Register()
        {
            serviceClient.Invoke(ClientToServerCommands.Register, null);
        }

        public void Unregister()
        {
            serviceClient.Invoke(ClientToServerCommands.Unregister, null);
        }

        public LightweightTask CheckoutTask()
        {
            JObject obj = serviceClient.Invoke(GetTask, null);
            if(obj != null)
            {
                return obj.ToObject<LightweightTask>();
            }
            return null;
        }

        public void CheckinTask(LightweightTask task)
        {
            JObject obj = new JObject(task);
            serviceClient.Invoke(FinishTask, obj);
        }

        public void AbandonTask(LightweightTask task)
        {
            JObject obj = new JObject(task);
            serviceClient.Invoke(ClientToServerCommands.AbandonTask, obj);
        }
    }
}
