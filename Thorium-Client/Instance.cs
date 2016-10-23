using System;
using Thorium_Shared;

namespace Thorium_Client
{
    public class Instance : MarshalByRefObject, IInstance
    {
        public void Ping()
        {
            
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }
    }
}
