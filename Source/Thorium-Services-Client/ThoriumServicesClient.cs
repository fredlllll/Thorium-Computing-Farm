using System;
using Thorium_Net;
using Thorium_Services_Shared;

namespace Thorium_Services_Client
{
    public class ThoriumServicesClient
    {
        public void RegisterService(ServiceDefinition serviceDefinition, ServiceTenancy tenancy = ServiceTenancy.Local)
        {

        }

        public ServiceDefinition GetServiceInfo(string name)
        {
            throw new NotImplementedException();
        }

        public IServiceInvoker GetServiceInvoker(string serviceName, ServiceTenancy tenancy = ServiceTenancy.Both, Type wantedInterfaceType = null)
        {
            /*
            TODO:
            get service definition from somewhere
            select an interface
            create invoker for said interface
            return invoker
    */
            throw new NotImplementedException();
        }
    }
}
