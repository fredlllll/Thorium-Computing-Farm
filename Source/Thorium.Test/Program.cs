using System;
using Newtonsoft.Json.Linq;
using Thorium.Net.ServiceHost;
using Thorium.Net.ServiceHost.InvokationHandlers;
using Thorium.Net.ServiceHost.InvokationReceivers;
using Thorium.Net.ServiceHost.Invokers;
using Thorium.Net.ServiceHost.Proxying;

namespace Thorium.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost sh = new ServiceHost();

            Service service = new Service();

            ObjectInvokationHandler<IServiceInterface> oih = new ObjectInvokationHandler<IServiceInterface>(service);
            sh.RegisterInvokationHandler(oih);

            Config.Config config = new Config.Config(new JObject() { ["port"] = 7677, ["host"] = "localhost" });
            HttpServiceInvokationReceiver ir = new HttpServiceInvokationReceiver(config);
            sh.RegisterInvokationReceiver(ir);
            sh.Start();

            HttpServiceInvoker iv = new HttpServiceInvoker(config);

            IServiceInterface proxy = ProxyFactory.CreateInstance<IServiceInterface>(iv);

            Console.WriteLine(proxy.Concatenate(56, " sheep"));
            Console.ReadKey();
        }
    }

    public class Service : IServiceInterface
    {
        public JToken Concatenate(int arg, string arg2)
        {
            return arg + arg2;
        }
    }

    public interface IServiceInterface
    {
        JToken Concatenate(int arg, string arg2);
    }
}
