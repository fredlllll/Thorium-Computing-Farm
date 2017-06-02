/*using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Services.Server
{
    public class DataPackageProviderServer : IDataPackageProviderServer
    {
        DirectoryInfo packageDirectory = new DirectoryInfo("DataPackageProviderPackages");

        StringDictionary packages = new StringDictionary();

        public void RegisterPackage(string name, byte[] blob)
        {
            lock(packages)
            {
                string fname = Util.GetRandomString(25);
                packages[name] = fname;
                File.WriteAllBytes(Path.Combine(packageDirectory.FullName, fname), blob);
            }
        }

        public void RegisterPackage(string name, DirectoryInfo directory)
        {
            lock(packages)
            {
                string fname = Util.GetRandomString(25);
                packages[name] = fname;
                ZipFile.CreateFromDirectory(directory.FullName, Path.Combine(packageDirectory.FullName, fname));
            }
        }

        public void UnregisterPackage(string name)
        {
            lock(packages)
            {
                string fname = packages[name];
                File.Delete(Path.Combine(packageDirectory.FullName, fname));
                packages.Remove(name);
            }
        }

        public byte[] GetPackage(string name)
        {
            lock(packages)
            {
                if(packages.ContainsKey(name))
                {
                    string fname = packages[name];
                    return File.ReadAllBytes(Path.Combine(packageDirectory.FullName, fname));
                }
            }
            return null;
        }
    }
}
*/