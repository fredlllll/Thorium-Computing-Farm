using System;
using System.ServiceModel;
using System.Threading;

namespace Thorium.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding b = new NetTcpBinding();

            var s = new Service();

            var address = "net.tcp://localhost:8100/test";
            Console.WriteLine("hosting " + s + " on " + address);

            Uri wcfAddress = new Uri(address);
            var host = new ServiceHost(s, wcfAddress);
            host.Open();


            EndpointAddress endpointAddress = new EndpointAddress("net.tcp://localhost:8100/test");

            var cb = new Callback();
            var channelFactory = new DuplexChannelFactory<IService>(cb, new NetTcpBinding(), endpointAddress);
            var serviceInstance = channelFactory.CreateChannel();

            serviceInstance.DoMe();

            Thread.Sleep(5000);
        }

        [ServiceContract(CallbackContract = typeof(ICallback), SessionMode = SessionMode.Required)]
        interface IService
        {
            [OperationContract(IsInitiating = true, IsOneWay = true)]
            void DoMe();
        }

        [ServiceContract]
        interface ICallback
        {
            [OperationContract]
            void DidMe();
        }

        class Callback : ICallback
        {
            public void DidMe()
            {
                Console.WriteLine("didme");
            }
        }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Reentrant)]
        class Service :IService
        {
            
            public void DoMe()
            {
                Console.WriteLine("dome");
                var client = OperationContext.Current.GetCallbackChannel<ICallback>();
                client.DidMe();
            }
        }

        
    }
}
