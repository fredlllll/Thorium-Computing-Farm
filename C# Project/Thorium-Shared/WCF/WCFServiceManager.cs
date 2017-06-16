using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.Services;
using static Thorium_Shared.SharedStatics;

namespace Thorium_Shared.WCF
{
    public class WCFServiceManager
    {
        static WCFServiceManager instance = default(WCFServiceManager);
        public static WCFServiceManager Instance
        {
            get
            {
                if(instance == default(WCFServiceManager))
                {
                    instance = new WCFServiceManager();
                }
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
        } = 8100;

        Binding binding;

        Dictionary<Type, WCFServiceInfo> serviceInstances = new Dictionary<Type, WCFServiceInfo>();
        Dictionary<IService, WCFServiceHostingInfo> serviceHosts = new Dictionary<IService, WCFServiceHostingInfo>();

        private WCFServiceManager()
        {
            var b = new NetTcpBinding();
            binding = b;

            /*b.Security.Message.ClientCredentialType = MessageCredentialType.IssuedToken;
            b.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
            b.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;*/
        }

        public InterfaceType GetServiceInstance<InterfaceType>(string remotePath, IService callbackInstance = null, string remoteHost = null) where InterfaceType : IService
        {
            WCFServiceInfo info;
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
            Logger.Log("getting service on " + endpointAddress);

            info = new WCFServiceInfo();
            if(callbackInstance == null)
            {
                var channelFactory = new ChannelFactory<InterfaceType>(binding, endpointAddress);
                channelFactory.Open();
                info.channelFactory = channelFactory;
                info.serviceInstance = channelFactory.CreateChannel();
            }
            else
            {
                var channelFactory = new DuplexChannelFactory<InterfaceType>(callbackInstance, binding, endpointAddress);
                channelFactory.Open();
                info.channelFactory = channelFactory;
                info.serviceInstance = channelFactory.CreateChannel();
                info.callbackInstance = callbackInstance;
            }

            return (InterfaceType)info.serviceInstance;
        }

        public void FreeServiceInstance<InterfaceType>()
        {
            WCFServiceInfo info;
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
                path = Utils.GetRandomString(25);
            }

            var address = "net.tcp://localhost:" + Port + "/" + path;
            Logger.Log("hosting " + serviceInstance + " on " + address);

            WCFServiceHostingInfo info = new WCFServiceHostingInfo();
            info.serviceInstance = serviceInstance;
            info.path = path;

            Uri wcfAddress = new Uri(address);
            info.serviceHost = new ServiceHost(serviceInstance, wcfAddress);
            //info.serviceHost.AddServiceEndpoint(typeof(InterfaceType), tcpBinding, wcfAddress);
            info.serviceHost.Open();

            serviceHosts[serviceInstance] = info;
            return path;
        }

        /*public string GetHostedInstancePath(IService serviceInstance)
        {
            WCFServiceHostingInfo info;
            if(serviceHosts.TryGetValue(serviceInstance, out info))
            {
                return info.path;
            }
            return null;
        }*/

        public void UnhostServiceInstance(IService serviceInstance)
        {
            WCFServiceHostingInfo info;
            if(serviceHosts.TryGetValue(serviceInstance, out info))
            {
                info.serviceHost.Close();
                serviceHosts.Remove(serviceInstance);
            }
        }
    }
}
