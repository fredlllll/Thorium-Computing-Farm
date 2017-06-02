using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using Thorium_Shared;
using Thorium_Shared.WCF;

namespace Thorium_Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ThoriumServerInterfaceForClient : IThoriumServerInterfaceForClient
    {
        ThoriumServer server;

        public ThoriumServerInterfaceForClient(ThoriumServer server)
        {
            this.server = server;
        }

        public void RegisterClient()
        {
            var client = OperationContext.Current.GetCallbackChannel<IThoriumClientInterfaceForServer>();
            server.ClientManager.RegisterClient(client);
        }

        public void UnregisterClient()
        {
            var client = OperationContext.Current.GetCallbackChannel<IThoriumClientInterfaceForServer>();
            server.ClientManager.UnregisterClient(client);
        }

        public TaskInformation GetFreeTaskInformation()
        {
            var client = OperationContext.Current.GetCallbackChannel<IThoriumClientInterfaceForServer>();
            var retVal = server.JobManager.GetFreeTask();
            //TODO: make not of which client processes which task (and vice versa)
            return retVal;
        }

        /*public string GetServicePath(Type interfaceType)
        {
            var service = ServiceManager.Instance.GetService(interfaceType);
            var address = WCFServiceManager.Instance.HostServiceInstance(service);
            return address;
        }*/

        public void SignalTaskAborted(string jobID, string taskID, string reason)
        {
            Console.WriteLine("Task Aborted. Reason: " + reason);
            server.JobManager.SignalTaskAborted(jobID, taskID, reason);
        }

        public void SignalTaskFinished(string jobID, string taskID)
        {
            server.JobManager.SignalTaskFinished(jobID, taskID);
        }
    }
}
