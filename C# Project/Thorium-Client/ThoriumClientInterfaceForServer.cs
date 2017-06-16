using System;
using System.ServiceModel;
using Thorium_Shared;

namespace Thorium_Client
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class ThoriumClientInterfaceForServer : IThoriumClientInterfaceForServer
    {
        ThoriumClient client;

        public ThoriumClientInterfaceForServer(ThoriumClient client)
        {
            this.client = client;
        }

        public string GetCurrentTaskID()
        {
            return client.CurrentTask.GetID();
        }

        public string GetCurrentTaskJobID()
        {
            return client.CurrentTask.GetJobID();
        }

        public string GetID()
        {
            return client.ID;
        }

        public void AbortTask(string ID)
        {
            client.AbortTask(ID);
        }

        public void Ping()
        {
            //dummy function to execute ping
        }
    }
}
