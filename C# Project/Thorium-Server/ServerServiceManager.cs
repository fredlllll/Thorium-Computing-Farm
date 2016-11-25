using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;
using Thorium_Shared.Services;

namespace Thorium_Server
{
    public class ServerServiceManager : AServiceManager<IServerService>
    {
        public ServerServiceManager()
        {
            SharedData.Set(ServerConfigConstants.SharedDataID_ServerServiceManager, this);
        }

        protected override IServerService GetNewInstance(Type type)
        {
            return (IServerService)Activator.CreateInstance(type);
        }
    }
}
