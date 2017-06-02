using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.Services;

namespace Thorium_Shared.WCF
{
    public class WCFServiceInfo
    {
        public ChannelFactory channelFactory;
        public IService serviceInstance;
        public IService callbackInstance;
        public int referenceCount = 1;
    }
}
