using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public interface IThoriumServerInterfaceForClient
    {
        bool RegisterInstance(IInstance instance);
        void UnregisterInstance(IInstance instance);

        SubJob GetSubJob();
        void FinishSubJob(SubJob sj);

        string GetRandomStorageServerAddress();
    }
}
