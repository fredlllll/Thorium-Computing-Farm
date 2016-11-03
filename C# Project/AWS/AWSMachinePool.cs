using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.MachineManager;

namespace AWS
{
    public class AWSMachinePool : IMachinePool
    {
        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IMachineType MachineType
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
