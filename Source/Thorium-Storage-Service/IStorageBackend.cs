using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Storage_Service
{
    public interface IStorageBackend
    {
        IEnumerable<string> GetDataPackageKeys(string id);

        void CreateDataPackage(string id);

        void DeleteDataPackage(string id);

        void CreateFile(string dataPackage, string key, string sourcefile);

        void DeleteFile(string dataPackage, string key);

        void MakeFileAvailable(string dataPackage, string key, string destinationFile);
    }
}
