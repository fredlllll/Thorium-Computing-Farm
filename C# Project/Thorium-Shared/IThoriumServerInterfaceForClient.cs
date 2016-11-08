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

        Task GetJobPart(IThoriumClientInterfaceForServer instance);
        void FinishJobPart(Task sj);

        //string GetRandomStorageServerAddress();
    }
}
