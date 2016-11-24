using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.Services.Server;

namespace Thorium_Shared.Services.Client
{
    public class DataPackageProviderClient : AClientService
    {
        DataPackageProviderServer serverService;
        DirectoryInfo packageDir;
        StringDictionary packages = new StringDictionary();
        FileInfo infoFile;

        public DataPackageProviderClient()
        {
            serverService = (DataPackageProviderServer)SharedData.Get<IThoriumServerInterfaceForClient>(ClientConfigConstants.SharedDataID_ServerInterfaceForClient).GetService(typeof(DataPackageProviderServer));
            packageDir = new DirectoryInfo(Path.Combine(SharedData.Get<Config>(ClientConfigConstants.SharedDataID_ClientConfig).GetString(ClientConfigConstants.ConfigID_DataDirectory), "DataPackageProvider"));
            infoFile = new FileInfo(Path.Combine(packageDir.FullName, "DataPackageProviderClient.info"));
            Directory.CreateDirectory(packageDir.FullName);
            Load();
        }

        /// <summary>
        /// returns a package as binary blob and caches it locally
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte[] GetPackage(string name)
        {
            string fname;
            if(packages.ContainsKey(name))
            {
                fname = packages[name];
                return File.ReadAllBytes(Path.Combine(packageDir.FullName, fname));
            }
            fname = Util.GetRandomString(25);
            byte[] ba = serverService.GetPackage(name);
            File.WriteAllBytes(Path.Combine(packageDir.FullName, fname), ba);
            packages[name] = fname;
            Save();
            return ba;
        }

        /// <summary>
        /// returns the local file path to a package, or gets it first if its not yet cached
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetPackageFile(string name)
        {
            string fname;
            if(packages.ContainsKey(name))
            {
                fname = packages[name];
                return Path.Combine(packageDir.FullName, fname);
            }
            fname = Util.GetRandomString(25);
            byte[] ba = serverService.GetPackage(name);
            File.WriteAllBytes(Path.Combine(packageDir.FullName, fname), ba);
            packages[name] = fname;
            Save();
            return Path.Combine(packageDir.FullName, fname);
        }

        public void UnpackPackageIntoDirectory(string name, DirectoryInfo directory)
        {
            string fname = GetPackageFile(name);
            ZipFile.ExtractToDirectory(fname, directory.FullName);
        }

        void Load()
        {
            string[] lines = File.ReadAllLines(infoFile.FullName);
            packages.Clear();
            foreach(string s in lines)
            {
                string[] sa = s.Split('=');
                packages[sa[0]] = sa[1];
            }
        }

        void Save()
        {
            string[] lines = packages.OfType<KeyValuePair<string, string>>().Select((x) => { return x.Key + "=" + x.Value; }).ToArray();
            File.WriteAllLines(infoFile.FullName, lines);
        }
    }
}
