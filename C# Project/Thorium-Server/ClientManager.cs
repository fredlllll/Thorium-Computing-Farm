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
            clients[client.GetID()] = client;
            Console.WriteLine("Client Registered: " + client.GetID());
        }

        public void UnregisterClient(IThoriumClientInterfaceForServer client)
        {
            string id = GetClientID(client);
            if(id != default(string))
            {
                UnregisterClient(clients[id], null);
            }
        }

        string GetClientID(IThoriumClientInterfaceForServer client)
        {
            foreach(var kv in clients)
            {
                if(kv.Value == client)
                {
                    return kv.Key;
                }
            }
            return default(string);
        }

        void UnregisterClient(IThoriumClientInterfaceForServer client, string reason)
        {
            string id = GetClientID(client);
            if(id != default(string))
            {
                clients.TryRemove(id, out client);
                Console.WriteLine("Client Unregistered: " + id + " - " + (reason == null ? "Unregistered" : "Reason: " + reason));
            }
        }

        void PingRun()
        {
            bool running = true;
            while(running)
            {
                if(clients.Count > 0)
                {
                    foreach(var kv in clients)
                    {
                        try
                        {
                            kv.Value.Ping();
                        }
                        catch(ThreadInterruptedException)
                        {
                            running = false;
                            break;
                        }
                        catch(Exception)//is it socketexception? TODO: find out what exception is thrown if client dies
                        {
                            UnregisterClient(kv.Value, "Client Died!");
                        }
                        Thread.Sleep(100);
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
