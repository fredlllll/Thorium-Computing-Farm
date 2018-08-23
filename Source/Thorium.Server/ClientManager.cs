using System.Threading;
using NLog;
using Thorium.Server.Data;
using Thorium.Server.Data.Serializers;
using Thorium.Threading;
using Thorium.Data.Implementation;
using System;
using Thorium.Data;

namespace Thorium.Server
{
    public class ClientManager : RestartableThreadClass
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ClientSerializer serializer;

        public delegate void ClientStoppedRespondingHandler(Client client);
        public event ClientStoppedRespondingHandler ClientStoppedResponding;

        public ClientManager(ClientSerializer serializer) : base(false)
        {
            this.serializer = serializer;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Stop(int joinTimeoutms = -1)
        {
            base.Stop(joinTimeoutms);
        }

        public void RegisterClient(Client client)
        {
            serializer.Save(client.Id, client);
            logger.Info("Client Registered: " + client.IPAddress);
        }

        public void UnregisterClient(string id, string reason = null)
        {
            serializer.Delete(id);
            logger.Info("Client Unregistered: " + id + " - " + (reason == null ? "No Reason given" : "Reason: " + reason));
        }

        public void UnregisterClient(Client client, string reason = null)
        {
            UnregisterClient(client.Id, reason);
        }

        protected override void Run()
        {
            try
            {
                while(true)
                {
                    foreach(var c in serializer.LoadAll())
                    {
                        try
                        {
                            c.Ping();
                        }
                        catch
                        {
                            ClientStoppedResponding?.Invoke(c);
                            UnregisterClient(c.Id, "Client Died!");
                        }
                    }
                    Thread.Sleep(30000);
                }
            }
            catch(ThreadInterruptedException)
            {
                //ending
            }
        }

        public void ReturnFreeClient(string id)
        {
            serializer.UpdateStatus(id, ClientStatus.Idle);
        }

        public Client GetFreeClient()
        {
            return serializer.CheckoutClient();
        }
    }
}
