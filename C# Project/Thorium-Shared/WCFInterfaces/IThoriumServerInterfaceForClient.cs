using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.Services;

namespace Thorium_Shared
{
    [ServiceContract]
    public interface IThoriumServerInterfaceForClient
    {
        [OperationContract]
        void RegisterClient(IThoriumClientInterfaceForServer instance);//place for doing client authentication, just so no random people can get access to your data TODO
        [OperationContract]
        void UnregisterClient(IThoriumClientInterfaceForServer instance);
        [OperationContract]
        ITask GetTask(IThoriumClientInterfaceForServer instance);
        [OperationContract]
        void ReturnUnfinishedTask(ITask task, string reason);
        [OperationContract]
        void TurnInTask(ITask task);

        [OperationContract]
        IServerService GetService(Type type);
    }
}
