using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using NLog;
using Thorium_Shared;

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

        Dictionary<IPAddress, Client> clients = new Dictionary<IPAddress, Client>();

        public delegate void ClientStoppedRespondingHandler(Client client);
        public event ClientStoppedRespondingHandler ClientStoppedResponding;

        public ClientManager() : base(false)
        {
        }

        public void RegisterClient(Client client)
        {
            lock(clients)
            {
                clients[client.IPAddress] = client;
            }
            logger.Info("Client Registered: " + client.IPAddress);
        }

        void UnregisterClient(Client client, string reason = null)
        {
            lock(clients)
            {
                clients.Remove(client.IPAddress);
            }
            logger.Info("Client Unregistered: " + client.IPAddress + " - " + (reason == null ? "No Reason given" : "Reason: " + reason));
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
