using System.IO;
using System.ServiceModel;

namespace Thorium_Shared.Services.Server
{
    [ServiceContract]
    public interface IDataPackageProviderServer : IService
    {
        [OperationContract]
        void RegisterPackage(string name, byte[] blob);

        [OperationContract]
        void RegisterPackage(string name, DirectoryInfo directory);

        [OperationContract]
        void UnregisterPackage(string name);

        [OperationContract]
        byte[] GetPackage(string name);
    }
}