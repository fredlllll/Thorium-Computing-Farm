using System;
using Newtonsoft.Json.Linq;
using Thorium_Shared;
using static Thorium_Shared.Net.ClientToServerCommands;
using NLog;
using Thorium_Shared.Net.ServicePoint;

namespace Thorium_Client
{
    public class ServerInterface
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        TCPServiceInvoker serviceClient;
        public ServerInterface(string host, ushort port)
        {
            serviceClient = new TCPServiceInvoker(host, port);
        }

        public void InvokeRegister(string id)
        {
            serviceClient.Invoke(Register, new JObject() { ["ip"] = Utils.GetExternalIP(), ["id"] = id });
        }

        public void InvokeUnregister(string id)
        {
            try
            {
                serviceClient.Invoke(Unregister, new JObject() { ["id"] = id });
            }
            catch(Exception ex)
            {
                logger.Info("exception when unregistering:");
                logger.Info(ex);
            }
        }

        public LightweightTask InvokeCheckoutTask()
        {
            JToken result = serviceClient.Invoke(CheckoutTask, null);
            if(result is JObject obj)
            {
                return obj.ToObject<LightweightTask>();
            }
            return null;
        }

        public void InvokeTurnInTask(LightweightTask task)
        {
            JObject arg = new JObject
            {
                ["id"] = task.ID
            };
            serviceClient.Invoke(TurnInTask, arg);
        }

        public void InvokeAbandonTask(LightweightTask task)
        {
            JObject arg = new JObject
            {
                ["id"] = task.ID
            };
            serviceClient.Invoke(AbandonTask, arg);
        }
    }
}
