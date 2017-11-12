using System.Collections.Generic;
using System.IO;
using static Thorium_Storage_Service.FileSystemStorageBackendConfig;

namespace Thorium_Storage_Service
{
    public class FileSystemStorageBackend : IStorageBackend
    {
        public FileSystemStorageBackend()
        {
            Directory.CreateDirectory(StorageDirectory);
        }

        public void CreateDataPackage(string id)
        {
            Directory.CreateDirectory(Path.Combine(StorageDirectory, id));
        }

        public void CreateFile(string dataPackage, string key, string sourcefile)
        {
            string file = Path.Combine(StorageDirectory, dataPackage, key);
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            File.Copy(sourcefile, file, true);
        }

        public void DeleteDataPackage(string id)
        {
            Directory.Delete(Path.Combine(StorageDirectory, id), true);
        }

        public void DeleteFile(string dataPackage, string key)
        {
            File.Delete(Path.Combine(StorageDirectory, dataPackage, key));
        }

        public IEnumerable<string> GetDataPackageKeys(string id)
        {
            return Directory.EnumerateFiles(Path.Combine(StorageDirectory, id), "*", SearchOption.AllDirectories);
        }

        public void MakeFileAvailable(string dataPackage, string key, string destinationFile)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));
            File.Copy(Path.Combine(StorageDirectory, dataPackage, key), destinationFile, true);
        }
    }
}
