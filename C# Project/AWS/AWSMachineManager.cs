using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.MachineManager;

namespace AWS
{
    public class AWSMachineManager : IMachineManager
    {
        //TODO: get access to aws to actually know whats up
        public void Free(int count, IMachinePool pool)
        {
            throw new NotImplementedException();
        }

        public void Request(int count, IMachinePool pool)
        {
            throw new NotImplementedException();
        }
    }
}
