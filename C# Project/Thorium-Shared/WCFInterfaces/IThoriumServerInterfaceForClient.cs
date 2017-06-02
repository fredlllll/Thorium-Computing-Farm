using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.Services;

namespace Thorium_Shared
{
    [ServiceContract(CallbackContract = typeof(IThoriumClientInterfaceForServer), SessionMode = SessionMode.Required)]
    public interface IThoriumServerInterfaceForClient : IService
    {
        /*[PrincipalPermission(SecurityAction.Demand, Role = "Client")]
        [OperationContract(IsInitiating = true, IsTerminating = false)]
        void Authenticate();

        [PrincipalPermission(SecurityAction.Demand, Role = "Client")]
        [OperationContract(IsInitiating = false, IsTerminating = true)]
        void Deauthenticate();*/

        [OperationContract(IsInitiating = true, IsTerminating = false)]
        void RegisterClient();//place for doing client authentication, just so no random people can get access to your data TODO
        [OperationContract(IsInitiating = false, IsTerminating = true)]
        void UnregisterClient();
        [OperationContract]
        TaskInformation GetFreeTaskInformation();
        [OperationContract(IsOneWay = true)]
        void SignalTaskAborted(string jobID, string taskID, string reason);
        [OperationContract(IsOneWay = true)]
        void SignalTaskFinished(string jobID, string taskID);

        /*[OperationContract]
        string GetServicePath(Type interfaceType);*/
    }
}
