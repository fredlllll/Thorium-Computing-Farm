using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Thorium_Shared.Services
{
    public class ServiceManager : IServiceProvider
    {
        class ServiceInfo
        {
            public Type interfaceType;
            public Type implementationType;
        }

        public static ServiceManager Instance
        {
            get;
        } = new ServiceManager();

        Dictionary<Type, ServiceInfo> services = new Dictionary<Type, ServiceInfo>();

        /// <summary>
        /// returns an instance of a service
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        public U GetService<U>() where U : class, IService
        {
            return (U)GetService(typeof(U));
        }

        /// <summary>
        /// returns an instance of a service
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public IService GetService(Type interfaceType)
        {
            ServiceInfo info;
            if(services.TryGetValue(interfaceType, out info))
            {
                return (IService)DependencyInjection.Kernel.Get(interfaceType);
            }
            return default(IService);
        }

        public void RegisterImplementation(Type interfaceType, Type implementationType)
        {
            ServiceInfo info;
            if(services.TryGetValue(interfaceType, out info))
            {
                throw new Exception("a service can only be registered once");
            }
            info = new ServiceInfo();
            info.interfaceType = interfaceType;
            info.implementationType = implementationType;
            DependencyInjection.Kernel.Bind(interfaceType).To(implementationType).InSingletonScope();
            services[interfaceType] = info;
        }
    }
}
