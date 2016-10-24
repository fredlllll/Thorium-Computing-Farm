using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Server
{
    public class ThoriumClientServerInterface : MarshalByRefObject, IThoriumServerInterfaceForClient
    {
        ThoriumServer server;

        public override object InitializeLifetimeService()
        {
            return null; //infinite lease lifetime
        }

        public ThoriumClientServerInterface(ThoriumServer server)
        {
            this.server = server;
        }

        public bool RegisterInstance(IInstance instance)
        {
            return server.InstanceManager.RegisterInstance(instance);
        }

        public void UnregisterInstance(IInstance instance)
        {
            server.InstanceManager.UnregisterInstance(instance);
        }

        public SubJob GetSubJob()
        {
            return server.GetSubJob();
        }

        public void FinishSubJob(SubJob sj) {
            server.Jobs[sj.JobID].FinishSubJob(sj);
        }

        public string GetRandomStorageServerAddress()
        {
            throw new NotImplementedException();
        }
    }
}
