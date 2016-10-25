using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Server
{
    public class ClientManager
    {
        public int MaxClients { get; set; }
        int ClientCount
        {
            get
            {
                return requests.Count + clients.Count;
            }
        }
        ConcurrentQueue<ClientInstanceRequest> requests = new ConcurrentQueue<ClientInstanceRequest>();
        ConcurrentDictionary<string, IThoriumClientInterfaceForServer> clients = new ConcurrentDictionary<string, IThoriumClientInterfaceForServer>();

        public IThoriumClientInterfaceForServer GetClient(string id)
        {
            return clients[id];
        }

        public int RequestClientInstances(int count)
        {
            count = Math.Min(count, MaxClients - ClientCount);//cant request more than allowed
            for(int i = 0; i < count; i++)
            {
                ClientInstanceRequest req = new ClientInstanceRequest(this);
                requests.Enqueue(req);
            }
            return count;
        }

        public bool RegisterClient(IThoriumClientInterfaceForServer client)
        {
            ClientInstanceRequest req = null;
            if(requests.TryDequeue(out req))
            {
                req.Satisfy();
                clients[client.ID] = client;
                return true;
            }
            else
            {//if too many instances we just shut down this one again
                return false;
            }
        }

        public void UnregisterClient(IThoriumClientInterfaceForServer client)
        {
            clients.TryRemove(client.ID, out client);
        }
    }
}
