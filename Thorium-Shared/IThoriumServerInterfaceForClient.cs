using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public interface IThoriumServerInterfaceForClient
    {
        bool RegisterClient(IThoriumClientInterfaceForServer instance);
        void UnregisterClient(IThoriumClientInterfaceForServer instance);

        JobPart GetJobPart(IThoriumClientInterfaceForServer instance);
        void FinishJobPart(JobPart sj);

        //string GetRandomStorageServerAddress();
    }
}
