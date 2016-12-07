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
    public interface IThoriumServerInterfaceForClient : IService
    {
        [OperationContract]
        void RegisterClient(string clientID);//place for doing client authentication, just so no random people can get access to your data TODO
        [OperationContract]
        void UnregisterClient(string clientID);
        [OperationContract]
        ITask GetTask(string clientID);
        [OperationContract]
        void ReturnUnfinishedTask(ITask task, string reason);
        [OperationContract]
        void TurnInTask(ITask task);

        [OperationContract]
        string GetServicePath(Type interfaceType);
    }
}
