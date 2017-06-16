using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Threading;
using Codolith.Serialization;
using Codolith.Serialization.Formatters;
using Thorium_Shared;
using Thorium_Shared.Jobtypes.SimpleExecution;

namespace Thorium_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding b = new NetTcpBinding();

            var s = new service();

            var address = "net.tcp://localhost:8100/test";
            Console.WriteLine("hosting " + s + " on " + address);

            Uri wcfAddress = new Uri(address);
            var host = new ServiceHost(s, wcfAddress);
            host.Open();


            EndpointAddress endpointAddress = new EndpointAddress("net.tcp://localhost:8100/test");

            var cb = new callback();
            var channelFactory = new DuplexChannelFactory<iservice>(cb, new NetTcpBinding(), endpointAddress);
            var serviceInstance = channelFactory.CreateChannel();

            serviceInstance.doMe();

            Thread.Sleep(5000);
        }

        [ServiceContract(CallbackContract = typeof(icallback), SessionMode = SessionMode.Required)]
        interface iservice
        {
            [OperationContract(IsInitiating = true, IsOneWay = true)]
            void doMe();
        }

        [ServiceContract]
        interface icallback
        {
            [OperationContract]
            void didMe();
        }

        class callback : icallback
        {
            public void didMe()
            {
                Console.WriteLine("didme");
            }
        }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Reentrant)]
        class service :iservice
        {
            
            public void doMe()
            {
                Console.WriteLine("dome");
                var client = OperationContext.Current.GetCallbackChannel<icallback>();
                client.didMe();
            }
        }

        
    }
}
