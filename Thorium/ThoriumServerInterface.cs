using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Server
{
    public class ThoriumServerInterface : MarshalByRefObject, IThoriumServerInterface
    {
        public ThoriumServerInterface(ThoriumServer server)
        {

        }

        public void RegisterInstance(IInstance instance)
        {

        }

        public void UnregisterInstance(IInstance instance)
        {

        }
    }
}
