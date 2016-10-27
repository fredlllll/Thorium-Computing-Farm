using System;
using Thorium_Shared;

namespace Thorium_Client
{
    public class ThoriumClientInterfaceForServer : MarshalByRefObject, IThoriumClientInterfaceForServer
    {
        string id = Util.GetRandomID();
        public string ID
        {
            get
            {
                return id;
            }
        }

        public void AbortJobPart(string ID)
        {
            throw new NotImplementedException();
        }

        public void Ping()
        {
            //dummy function to execute ping
        }
    }
}
