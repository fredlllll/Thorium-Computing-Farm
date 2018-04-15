using System.Collections.Generic;
using Thorium.Server.Data.Serializers;

namespace Thorium.Server
{
    public class ClientTaskRelationManager
    {
        Dictionary<string, ClientTaskRelation> byClient = new Dictionary<string, ClientTaskRelation>();
        Dictionary<string, ClientTaskRelation> byTask = new Dictionary<string, ClientTaskRelation>();
        private readonly ClientTaskRelationSerializer serializer;

        public ClientTaskRelationManager(ClientTaskRelationSerializer serializer)
        {
            this.serializer = serializer;
        }

        public void Start()
        {
            //load
            lock(byClient)
            {
                lock(byTask)
                {
                    foreach(var ctr in serializer.LoadAll())
                    {
                        byClient[ctr.Client] = ctr;
                        byTask[ctr.Task] = ctr;
                    }
                }
            }
        }

        public void Stop()
        {
            //save
            lock(byClient)
            {
                lock(byTask)
                {
                    foreach(var ctr in byClient)
                    {
                        serializer.Save(ctr.Key, ctr.Value);
                    }
                }
            }
        }

        public void Add(ClientTaskRelation relation)
        {
            serializer.Save(relation.Client, relation);
            lock(byClient)
            {
                lock(byTask)
                {
                    byClient[relation.Client] = relation;
                    byTask[relation.Task] = relation;
                }
            }
        }

        public void Remove(ClientTaskRelation relation)
        {
            serializer.Delete(relation.Client);
            lock(byClient)
            {
                lock(byTask)
                {
                    byClient.Remove(relation.Client);
                    byTask.Remove(relation.Task);
                }
            }
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
