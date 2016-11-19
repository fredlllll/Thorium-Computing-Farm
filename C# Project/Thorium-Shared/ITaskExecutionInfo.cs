using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.ExecutionActions;

namespace Thorium_Shared
{
    public interface ITaskExecutionInfo
    {
        void Setup();
        void Run();
        byte[] GetResultZip();
    }
}
