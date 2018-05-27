using System;
using Newtonsoft.Json.Linq;
using Thorium.Shared;
using static Thorium.Shared.Net.ClientToServerCommands;
using NLog;
using Thorium.Net;
using Thorium.Net.ServiceHost.Invokers;

namespace Thorium.Client
{
    public class ServerInterface
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly TCPServiceInvoker serviceClient;

        public ThoriumClient Client { get; }

        public ServerInterface(string host, ushort port, ThoriumClient client)
        {
            serviceClient = new TCPServiceInvoker(host, port);
            Client = client;
        }

        public void InvokeRegister()
        {
            serviceClient.Invoke(Register, new JObject() { ["ip"] = NetUtils.GetExternalIP(), ["clientId"] = Client.ID });
        }

        public void InvokeUnregister()
        {
            try
            {
                serviceClient.Invoke(Unregister, new JObject() { ["clientId"] = Client.ID });
            }
            catch(Exception ex)
            {
                logger.Info("exception when unregistering:");
                logger.Info(ex);
            }
        }

        public LightweightTask InvokeCheckoutTask()
        {
            JToken result = serviceClient.Invoke(CheckoutTask, new JObject { ["clientId"] = Client.ID });
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
                ["clientId"] = Client.ID,
                ["taskId"] = task.ID
            };
            serviceClient.Invoke(TurnInTask, arg);
        }

        public void InvokeAbandonTask(LightweightTask task, string reason = null)
        {
            JObject arg = new JObject
            {
                ["clientId"] = Client.ID,
                ["taskId"] = task.ID
            };
            if(reason != null)
            {
                arg["reason"] = reason;
            }
            serviceClient.Invoke(AbandonTask, arg);
        }

        public void InvokeFailTask(LightweightTask task, string reason = null)
        {
            JObject arg = new JObject
            {
                ["clientId"] = Client.ID,
                ["taskId"] = task.ID
            };
            if(reason != null)
            {
                arg["reason"] = reason;
            }
            serviceClient.Invoke(FailTask, arg);
        }
    }
}
