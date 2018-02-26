using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.Net.ServicePoint;

namespace Thorium_Shared.Services
{
    public class ThoriumServices
    {
        public void RegisterService(ServiceDefinition serviceDefinition, ServiceTenancy tenancy = ServiceTenancy.Local)
        {

        }

        public ServiceDefinition GetServiceInfo(string name)
        {

        }

        public IServiceInvoker GetServiceInvoker(string serviceName, ServiceTenancy tenancy = ServiceTenancy.Both)
        {
            /*
            TODO:
            get service definition from somewhere
            select an interface
            create invoker for said interface
            return invoker
    */
        }
    }
}
