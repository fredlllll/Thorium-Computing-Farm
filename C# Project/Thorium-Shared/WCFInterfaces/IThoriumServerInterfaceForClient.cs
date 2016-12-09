using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.Services;

namespace Thorium_Shared
{
    [ServiceContract(CallbackContract = typeof(IThoriumClientInterfaceForServer))]
    public interface IThoriumServerInterfaceForClient : IService
    {
        [OperationContract]
        void RegisterClient();//place for doing client authentication, just so no random people can get access to your data TODO
        [OperationContract]
        void UnregisterClient();
        [OperationContract]
        ITask GetTask();
        [OperationContract]
        void ReturnUnfinishedTask(ITask task, string reason);
        [OperationContract]
        void TurnInTask(ITask task);

        [OperationContract]
        string GetServicePath(Type interfaceType);
    }
}
