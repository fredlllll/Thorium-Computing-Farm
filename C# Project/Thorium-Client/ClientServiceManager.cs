using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;
using Thorium_Shared.Services;

namespace Thorium_Client
{
    public class ClientServiceManager : AServiceManager<AClientService>
    {
        public ClientServiceManager()
        {
            SharedData.Set(ClientConfigConstants.SharedDataID_ClientServiceManager,this);
        }

        protected override AClientService GetNewInstance(Type type)
        {
            return (AClientService)Activator.CreateInstance(type);
        }
    }
}
