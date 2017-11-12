using System;
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
            Console.WriteLine("CreateDataPackage: " + id);
            string dir = Path.Combine(StorageDirectory, id);
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public void CreateFile(string dataPackage, string key, string sourcefile)
        {
            Console.WriteLine("CreateFile: " + dataPackage + "," + key + "," + sourcefile);
            string file = Path.Combine(StorageDirectory, dataPackage, key);
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            File.Copy(sourcefile, file, true);
        }

        public void DeleteDataPackage(string id)
        {
            Console.WriteLine("DeleteDataPackage: " + id);
            Directory.Delete(Path.Combine(StorageDirectory, id), true);
        }

        public void DeleteFile(string dataPackage, string key)
        {
            Console.WriteLine("DeleteFile: " + dataPackage + "," + key);
            File.Delete(Path.Combine(StorageDirectory, dataPackage, key));
        }

        public IEnumerable<string> GetDataPackageKeys(string id)
        {
            Console.WriteLine("GetDataPackageKeys: " + id);
            return Directory.EnumerateFiles(Path.Combine(StorageDirectory, id), "*", SearchOption.AllDirectories);
        }

        public void MakeFileAvailable(string dataPackage, string key, string destinationFile)
        {
            Console.WriteLine("MakeFileAvailable: " + dataPackage + "," + key + "," + destinationFile);
            Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));
            File.Copy(Path.Combine(StorageDirectory, dataPackage, key), destinationFile, true);
        }
    }
}
