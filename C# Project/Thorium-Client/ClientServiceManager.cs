using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;
using Thorium_Shared.Services;

namespace Thorium_Client
{
    public class ClientServiceManager : AServiceManager<IClientService>
    {
        public ClientServiceManager()
        {
            SharedData.Set(ClientConfigConstants.SharedDataID_ClientServiceManager,this);
        }

        protected override IClientService GetNewInstance(Type type)
        {
            return (IClientService)Activator.CreateInstance(type);
        }
    }
}
