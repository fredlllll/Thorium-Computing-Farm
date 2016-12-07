using System.IO;
using System.ServiceModel;

namespace Thorium_Shared.Services.Client
{
    [ServiceContract]
    public interface IDataPackageProviderClient : IService
    {
        [OperationContract]
        byte[] GetPackage(string name);
        [OperationContract]
        string GetPackageFile(string name);
        [OperationContract]
        void UnpackPackageIntoDirectory(string name, DirectoryInfo directory);
    }
}