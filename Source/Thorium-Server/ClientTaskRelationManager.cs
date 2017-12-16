using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Server
{
    public class ClientTaskRelationManager
    {
        Dictionary<string, ClientTaskRelation> byClient = new Dictionary<string, ClientTaskRelation>();
        Dictionary<string, ClientTaskRelation> byTask = new Dictionary<string, ClientTaskRelation>();

        public void Start()
        {
            //load
        }

        public void Stop()
        {
            //save
        }

        public void Add(ClientTaskRelation relation)
        {
            byClient[relation.Client] = relation;
            byTask[relation.Task] = relation;
        }

        public void Remove(ClientTaskRelation relation)
        {
            byClient.Remove(relation.Client);
            byTask.Remove(relation.Task);
        }

        public void RemoveByClient(string client)
        {
            Remove(byClient[client]);
        }

        public void RemoveByTask(string task)
        {
            Remove(byTask[task]);
        }
    }
}
