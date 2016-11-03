using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.MachineManager
{
    public interface IMachinePool
    {
        IMachineType MachineType { get; }
        int Count { get; }
    }
}
