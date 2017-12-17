using System.Threading;
using NLog;
using Thorium_Shared;
using Thorium_Shared.Data.Serializers;

namespace Thorium_Server
{
    public class ClientManager : RestartableThreadClass
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ClientSerializer serializer;

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

        public override void Stop()
        {
            base.Stop();
        }

        public void RegisterClient(Client client)
        {
            serializer.Save(client.ID, client);
            logger.Info("Client Registered: " + client.IPAddress);
        }

        public void UnregisterClient(string id, string reason = null)
        {
            serializer.Delete(id);
            logger.Info("Client Unregistered: " + id + " - " + (reason == null ? "No Reason given" : "Reason: " + reason));
        }

        public void UnregisterClient(Client client, string reason = null)
        {
            UnregisterClient(client.ID, reason);
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
                            UnregisterClient(c.ID, "Client Died!");
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
    }
}
