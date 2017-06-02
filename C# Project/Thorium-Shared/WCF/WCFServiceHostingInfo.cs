using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.Services;

namespace Thorium_Shared.WCF
{
    public class WCFServiceHostingInfo
    {
        public ServiceHost serviceHost;
        public IService serviceInstance;
        public string path;
    }
}
