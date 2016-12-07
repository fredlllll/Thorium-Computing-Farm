using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.Services;

namespace Thorium_Shared
{
    public class WCFServiceManager
    {
        class ServiceInfo
        {
            public ChannelFactory channelFactory;
            public IService serviceInstance;
            public int referenceCount = 1;
        }

        class ServiceHostingInfo
        {
            public ServiceHost serviceHost;
            public IService serviceInstance;
            public string path;
        }

        static WCFServiceManager instance = new WCFServiceManager(); //lazy static initialization
        public static WCFServiceManager Instance
        {
            get
            {
                return instance;
            }
        }

        public string RemoteHost
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        } = 8200;

        Dictionary<Type, ServiceInfo> serviceInstances = new Dictionary<Type, ServiceInfo>();
        Dictionary<IService, ServiceHostingInfo> serviceHosts = new Dictionary<IService, ServiceHostingInfo>();
        NetTcpBinding tcpBinding = new NetTcpBinding();

        private WCFServiceManager() { }

        public InterfaceType GetServiceInstance<InterfaceType>(string remotePath, string remoteHost = null) where InterfaceType : IService
        {
            ServiceInfo info;
            Type T = typeof(InterfaceType);
            if(serviceInstances.ContainsKey(T))
            {
                info = serviceInstances[T];
                info.referenceCount++;
                return (InterfaceType)info.serviceInstance;
            }

            if(remoteHost == null)
            {
                remoteHost = RemoteHost;
            }

            EndpointAddress endpointAddress = new EndpointAddress("net.tcp://" + remoteHost + ":" + Port + "/" + remotePath);

            info = new ServiceInfo();
            //var channelFactory = new ChannelFactory<InterfaceType>(T.Name + "_Endpoint", endpointAddress);
            //var tcpBinding = new NetTcpBinding();
            var channelFactory = new ChannelFactory<InterfaceType>(tcpBinding, endpointAddress);
            //TODO: duplex @~@
            //var channelFactory = new DuplexChannelFactory<InterfaceType>(
            info.channelFactory = channelFactory;
            return (InterfaceType)(info.serviceInstance = channelFactory.CreateChannel());
        }

        public void FreeServiceInstance<InterfaceType>()
        {
            ServiceInfo info;
            Type T = typeof(InterfaceType);
            if(serviceInstances.TryGetValue(T, out info))
            {
                if(--(info.referenceCount) <= 0)
                {
                    info.channelFactory.Close();
                    serviceInstances.Remove(T);
                }
            }
        }

        /*public void ForceDisconnectServiceInstance<(object serviceInstance)
        {

        }*/

        public string HostServiceInstance(IService serviceInstance, string path = null)
        {
            if(string.IsNullOrEmpty(path))
            {
                path = Util.GetRandomString(25);
            }

            var address = "net.tcp://localhost:" + Port + "/" + path;

            ServiceHostingInfo info = new ServiceHostingInfo();
            info.serviceInstance = serviceInstance;
            info.path = path;

            Uri wcfAddress = new Uri(address);
            info.serviceHost = new ServiceHost(serviceInstance, wcfAddress);
            //info.serviceHost.AddServiceEndpoint(typeof(InterfaceType), tcpBinding, wcfAddress);
            info.serviceHost.Open();

            serviceHosts[serviceInstance] = info;
            return path;
        }

        public string GetHostedInstancePath(IService serviceInstance)
        {
            ServiceHostingInfo info;
            if(serviceHosts.TryGetValue(serviceInstance, out info))
            {
                return info.path;
            }
            return null;
        }

        public void UnhostServiceInstance(IService serviceInstance)
        {
            ServiceHostingInfo info;
            if(serviceHosts.TryGetValue(serviceInstance, out info))
            {
                info.serviceHost.Close();
                serviceHosts.Remove(serviceInstance);
            }
        }
    }
}
