using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public interface IThoriumServerInterfaceForClient
    {
        void RegisterClient(IThoriumClientInterfaceForServer instance);//place for doing client authentication, just so no random people can get access to your data TODO
        void UnregisterClient(IThoriumClientInterfaceForServer instance);

        Task GetTask(IThoriumClientInterfaceForServer instance);
        void ReturnUnfinishedTask(Task task, string reason);
        void TurnInTask(Task task, byte[] resultZip);
    }
}
