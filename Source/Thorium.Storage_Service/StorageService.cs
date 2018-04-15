﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NLog;
using Thorium_Config;
using Thorium_Reflection;

namespace Thorium_Storage_Service
{
    public static class StorageService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static Dictionary<string, string> cachedPackages = new Dictionary<string, string>();

        private static IStorageBackend storageBackend;

        static StorageService()
        {
            var config = ConfigFile.GetClassConfig();

            Type t = ReflectionHelper.GetType(config.StorageBackend);
            if(t == null)
            {
                logger.Error("could not find type: " + config.StorageBackend);
                throw new Exception("could not find type: " + config.StorageBackend);
            }
            ConstructorInfo ci = t.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            storageBackend = (IStorageBackend)ci.Invoke(new Type[] { });
        }

        /// <summary>
        /// makes a datapackage available in the targetdirectory. optionally it takes a <paramref name="postprocessingAction"/> that is called once to post process the download
        /// </summary>
        /// <param name="id">package id</param>
        /// <param name="targetDirectory">target directory</param>
        /// <param name="postprocessingAction">optional action that is used to process the downloaded package contents</param>
        public static void MakeDataPackageAvailable(string id, string targetDirectory, Action<string, string> postprocessingAction = null)
        {
            if(!cachedPackages.TryGetValue(id, out string packageCacheDir))
            {
                packageCacheDir = Path.Combine(Thorium_IO.Directories.TempDir, id + "_cache");
                string downloadTarget = packageCacheDir;
                if(postprocessingAction != null)
                {
                    downloadTarget = Path.Combine(Thorium_IO.Directories.TempDir, id + "_download");
                }

                Directory.CreateDirectory(downloadTarget);

                var keys = storageBackend.GetDataPackageKeys(id);
                foreach(var key in keys)
                {
                    string fileTarget = Path.Combine(downloadTarget, key);
                    logger.Debug("making " + key + " available at " + fileTarget);
                    storageBackend.MakeFileAvailable(id, key, fileTarget);
                }

                if(postprocessingAction != null)
                {
                    Directory.CreateDirectory(packageCacheDir);
                    postprocessingAction?.Invoke(downloadTarget, packageCacheDir);
                    Directory.Delete(downloadTarget, true);
                }

                cachedPackages[id] = packageCacheDir;
            }
            Directory.CreateDirectory(targetDirectory);
            Thorium_IO.Directory.CopyDirectory(packageCacheDir, targetDirectory);
        }

        public static void CreateDataPackage(string id, string sourceDirectory, bool deleteSourceAfterUpload = false)
        {
            sourceDirectory = Path.GetFullPath(sourceDirectory); //eliminate .. and such
            storageBackend.CreateDataPackage(id);
            var files = Directory.EnumerateFiles(sourceDirectory, "*", SearchOption.AllDirectories);
            foreach(var file in files)
            {
                string key = file.Replace(sourceDirectory, "");
                key = key.TrimStart(Path.DirectorySeparatorChar);
                if(Path.DirectorySeparatorChar != '/')
                {
                    key = key.Replace(Path.DirectorySeparatorChar, '/');
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
                key = key.TrimStart(Path.DirectorySeparatorChar);
                key = Path.Combine(taskID, key);
                if(Path.DirectorySeparatorChar != '/')
                {
                    key = key.Replace(Path.DirectorySeparatorChar, '/');
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
