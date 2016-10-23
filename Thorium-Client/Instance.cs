using System;
using Thorium_Shared;

namespace Thorium_Client
{
    public class Instance : MarshalByRefObject, IInstance
    {
        public void Ping()
        {
            //dummy function to execute ping
        }

        public void Shutdown()
        {
            ClientUtil.ShutdownSystem();
        }
    }
}
