using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Storage_Service
{
    public static class StorageService
    {
        private static IStorageBackend storageBackend;

        static StorageService()
        {
            Type t = Type.GetType(StorageServiceConfig.StorageBackend);
            ConstructorInfo ci = t.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            storageBackend = (IStorageBackend)ci.Invoke(new Type[] { });
        }

        public static void MakeDataPackageAvailable(string id, string targetDirectory)
        {
            var keys = storageBackend.GetDataPackageKeys(id);
            foreach(var key in keys)
            {
                storageBackend.MakeFileAvailable(id, key, Path.Combine(targetDirectory, key));
            }
        }

        public static void CreateDataPackage(string id, string sourceDirectory, bool deleteSourceAfterUpload = false)
        {
            sourceDirectory = Path.GetFullPath(sourceDirectory); //eliminate .. and such
            storageBackend.CreateDataPackage(id);
            var files = Directory.EnumerateFiles(sourceDirectory, "*", SearchOption.AllDirectories);
            foreach(var file in files)
            {
                string key = file.Replace(sourceDirectory, "");
                key = key.TrimStart(Path.PathSeparator);
                if(Path.PathSeparator != '/')
                {
                    key = key.Replace(Path.PathSeparator, '/');
                }
                storageBackend.CreateFile(id, key, file);
            }
            if(deleteSourceAfterUpload)
            {
                Directory.Delete(sourceDirectory, true);
            }
        }

        public static void DeleteDataPackage(string id)
        {
            storageBackend.DeleteDataPackage(id);
        }

        public static void DownloadResults(string jobID, string targetDirectory)
        {
            MakeDataPackageAvailable(jobID, targetDirectory);
        }

        public static void UploadResults(string jobID, string taskID, string sourceDirectory, bool deleteSourceAfterUpload = true)
        {
            sourceDirectory = Path.GetFullPath(sourceDirectory); //eliminate .. and such
            storageBackend.CreateDataPackage(jobID);
            var files = Directory.EnumerateFiles(sourceDirectory, "*", SearchOption.AllDirectories);
            foreach(var file in files)
            {
                string key = file.Replace(sourceDirectory, "");
                key = key.TrimStart(Path.PathSeparator);
                key = Path.Combine(taskID, key);
                if(Path.PathSeparator != '/')
                {
                    key = key.Replace(Path.PathSeparator, '/');
                }
                storageBackend.CreateFile(jobID, key, file);
            }
            if(deleteSourceAfterUpload)
            {
                Directory.Delete(sourceDirectory, true);
            }
        }

        public static void DeleteResults(string jobID)
        {
            storageBackend.DeleteDataPackage(jobID);
        }
    }
}
