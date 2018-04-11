using System;
using Newtonsoft.Json.Linq;
using Thorium_Net;

namespace Thorium_Services_Host
{
    public class ServicesHost
    {
        ServiceHost serviceHost;

        public ServicesHost()
        {
            serviceHost = new ServiceHost();

            Routine registerService = new Routine(nameof(registerService), RegisterService);
        }

        JToken RegisterService(JToken arg)
        {
            if(arg is JObject jo)
            {
                jo.Get<string>("id");
                jo.Get<string>("name");

                return true;
            }
            else
            {
                throw new ArgumentException("this method expects a JObject as argument");
            }
        }

        public void Start()
        {
            serviceHost.Start();
        }

        public void Stop()
        {
            serviceHost.Stop();
        }
    }
}
