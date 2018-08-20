using System.Threading;
using NLog;
using Thorium.Server.Data;
using Thorium.Server.Data.Serializers;
using Thorium.Threading;
using Thorium.Data.Implementation;
using System;

namespace Thorium.Server
{
    public class ClientManager : RestartableThreadClass
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ClientSerializer serializer;

        public delegate void ClientStoppedRespondingHandler(Client client);
        public event ClientStoppedRespondingHandler ClientStoppedResponding;

        MySqlDatabase database;

        public ClientManager() : base(false)
        {
        }

        public override void Start()
        {
            database = DataManager.GetNewDatabase();
            serializer = new ClientSerializer(database);
            base.Start();
        }

        public override void Stop(int joinTimeoutms = -1)
        {
            base.Stop(joinTimeoutms);
            database.Dispose();
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
            //TODO: this will be a bottleneck with many clients. have to find a way to make it work without locking the collection
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
