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
            serviceClient.Invoke(Register, new JObject() { ["ip"] = NetUtils.GetExternalIP(), ["clientId"] = Client.Id });
        }

        public void InvokeUnregister()
        {
            try
            {
                serviceClient.Invoke(Unregister, new JObject() { ["clientId"] = Client.Id });
            }
            catch(Exception ex)
            {
                logger.Info("exception when unregistering:");
                logger.Info(ex);
            }
        }

        public void InvokeTurnInTask(string taskId, string additionalInformation = "")
        {
            JObject arg = new JObject
            {
                ["clientId"] = Client.Id,
                ["taskId"] = taskId,
                ["additionalInformation"] = additionalInformation,
            };
            serviceClient.Invoke(TurnInTask, arg);
        }

        public void InvokeAbandonTask(string taskId, string additionalInformation = "")
        {
            JObject arg = new JObject
            {
                ["clientId"] = Client.Id,
                ["taskId"] = taskId,
                ["additionalInformation"] = additionalInformation,
            };
            serviceClient.Invoke(AbandonTask, arg);
        }

        public void InvokeFailTask(string taskId, string additionalInformation = "")
        {
            JObject arg = new JObject
            {
                ["clientId"] = Client.Id,
                ["taskId"] = taskId,
                ["additionalInformation"] = additionalInformation,
            };
            serviceClient.Invoke(FailTask, arg);
        }
    }
}
