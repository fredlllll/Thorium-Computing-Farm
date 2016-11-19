using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Thorium_Shared;

namespace Thorium_Server
{
    public class ClientManager
    {
        public int ClientCount
        {
            get
            {
                return clients.Count;
            }
        }
        Thread pingThread;
        ConcurrentDictionary<string, IThoriumClientInterfaceForServer> clients = new ConcurrentDictionary<string, IThoriumClientInterfaceForServer>();

        public ClientManager()
        {
            pingThread = new Thread(PingRun);
            pingThread.Start();
        }

        public IThoriumClientInterfaceForServer GetClient(string id)
        {
            return clients[id];
        }

        public void RegisterClient(IThoriumClientInterfaceForServer client)
        {
            clients[client.ID] = client;
            Console.WriteLine("Client Registered: " + client.ID);
        }

        public void UnregisterClient(IThoriumClientInterfaceForServer client)
        {
            UnregisterClient(client, null);
        }

        void UnregisterClient(IThoriumClientInterfaceForServer client, string reason)
        {
            clients.TryRemove(client.ID, out client);
            Console.WriteLine("Client Unregistered: " + client.ID + (reason == null ? "No Reason" : " Reason: " + reason));
        }

        void PingRun()
        {
            bool running = true;
            while(running)
            {
                foreach(var kv in clients)
                {
                    try
                    {
                        kv.Value.Ping();
                        Thread.Sleep(100);
                    }
                    catch(ThreadInterruptedException)
                    {
                        running = false;
                        break;
                    }
                    catch(Exception ex)//is it socketexception? TODO
                    {
                        UnregisterClient(kv.Value, "Client Died!");
                    }
                }
            }
        }
    }
}
