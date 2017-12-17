using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using NLog;
using Thorium_Shared;
using Thorium_Shared.Data.Serializers;

namespace Thorium_Server
{
    public class ClientManager : RestartableThreadClass
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public int ClientCount
        {
            get
            {
                return clients.Count;
            }
        }

        /// <summary>
        /// returns a snapshot of the registered clients
        /// </summary>
        public IEnumerable<Client> ClientsSnapshot
        {
            get
            {
                lock(clients)
                {
                    return clients.Select((x) => x.Value);
                }
            }
        }

        Dictionary<string, Client> clients = new Dictionary<string, Client>();
        private readonly ClientSerializer serializer;

        public delegate void ClientStoppedRespondingHandler(Client client);
        public event ClientStoppedRespondingHandler ClientStoppedResponding;

        public ClientManager(ClientSerializer serializer) : base(false)
        {
            this.serializer = serializer;
        }

        public override void Start()
        {
            //load
            lock(clients)
            {
                foreach(var c in serializer.LoadAll())
                {
                    clients.Add(c.ID, c);
                }
            }

            base.Start();
        }

        public override void Stop()
        {
            base.Stop();

            //save
            lock(clients)
            {
                foreach(var c in clients)
                {
                    serializer.Save(c.Key, c.Value);
                }
            }
        }

        public void RegisterClient(Client client)
        {
            serializer.Save(client.ID, client);
            lock(clients)
            {
                clients[client.ID] = client;
            }
            logger.Info("Client Registered: " + client.IPAddress);
        }

        public void UnregisterClient(string id, string reason = null)
        {
            UnregisterClient(clients[id], reason);
        }

        public void UnregisterClient(Client client, string reason = null)
        {
            lock(clients)
            {
                clients.Remove(client.ID);
            }
            serializer.Delete(client.ID);
            logger.Info("Client " + client.ID + " Unregistered: " + client.IPAddress + " - " + (reason == null ? "No Reason given" : "Reason: " + reason));
        }

        protected override void Run()
        {
            //TODO: this will be a bottleneck with many clients. have to find a way to make it work without locking the collection
            try
            {
                while(true)
                {
                    lock(clients)
                    {
                        if(clients.Count > 0)
                        {
                            foreach(var kv in clients)
                            {
                                try
                                {
                                    kv.Value.Ping();
                                }
                                catch(Exception)//is it socketexception? TODO: find out what exception is thrown if client dies
                                {
                                    ClientStoppedResponding?.Invoke(kv.Value);
                                    UnregisterClient(kv.Value, "Client Died!");
                                }
                            }
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
