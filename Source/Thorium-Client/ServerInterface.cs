using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Thorium_Shared;
using Thorium_Shared.Net.Comms;
using Thorium_Shared.Net;
using static Thorium_Shared.Net.ClientToServerCommands;
using NLog;

namespace Thorium_Client
{
    public class ServerInterface
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

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
            try
            {
                serviceClient.Invoke(ClientToServerCommands.Unregister, null);
            }
            catch(Exception ex)
            {
                logger.Info("exception when unregistering:");
                logger.Info(ex);
            }
        }

        public LightweightTask CheckoutTask()
        {
            JObject obj = serviceClient.Invoke(ClientToServerCommands.CheckoutTask, null);
            if(obj != null)
            {
                return obj.ToObject<LightweightTask>();
            }
            return null;
        }

        public void TurnInTask(LightweightTask task)
        {
            JObject arg = new JObject
            {
                ["id"] = task.ID
            };
            serviceClient.Invoke(ClientToServerCommands.TurnInTask, arg);
        }

        public void AbandonTask(LightweightTask task)
        {
            JObject arg = new JObject
            {
                ["id"] = task.ID
            };
            serviceClient.Invoke(ClientToServerCommands.AbandonTask, arg);
        }
    }
}
