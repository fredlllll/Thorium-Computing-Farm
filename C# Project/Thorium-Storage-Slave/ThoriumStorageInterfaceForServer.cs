using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;
using Thorium_Shared.WCF;

namespace Thorium_Storage_Slave
{
    public class ThoriumStorageInterfaceForServer : IService
    {
        public void CreateFile(string JobID, string ID, string extension, byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public void RemoveFile(string JobID, string ID)
        {
            throw new NotImplementedException();
        }
    }
}
