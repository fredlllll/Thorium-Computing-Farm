using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Server
{
    public class InstanceManager
    {
        public int MaxInstances { get; set; }
        int InstanceCount
        {
            get
            {
                return requests.Count + instances.Count;
            }
        }
        ConcurrentQueue<InstanceRequest> requests = new ConcurrentQueue<InstanceRequest>();
        ConcurrentDictionary<IInstance, byte> instances = new ConcurrentDictionary<IInstance, byte>();

        public int RequestInstances(int count)
        {
            count = Math.Min(count, MaxInstances - InstanceCount);//cant request more than allowed
            for(int i = 0; i < count; i++)
            {
                InstanceRequest req = new InstanceRequest(this);
                requests.Enqueue(req);
            }
            return count;
        }

        public bool RegisterInstance(IInstance instance)
        {
            InstanceRequest req = null;
            if(requests.TryDequeue(out req))
            {
                req.Satisfy();
                instances[instance] = 0;
                return true;
            }
            else
            {//if too many instances we just shut down this one again
                return false;
            }
        }

        public void UnregisterInstance(IInstance instance)
        {
            byte b;
            instances.TryRemove(instance, out b);
        }
    }
}
