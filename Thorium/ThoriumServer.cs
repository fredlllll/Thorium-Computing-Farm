using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Server
{
    public class ThoriumServer
    {
        TcpServerChannel tcpChannel;
        ThoriumServerInterface serverInterface;

        public InstanceManager instanceManager = new InstanceManager();

        public ThoriumServer(int instanceServerPort = 8100)
        {
            serverInterface = new ThoriumServerInterface(this);
            tcpChannel = new TcpServerChannel(instanceServerPort);
            ChannelServices.RegisterChannel(tcpChannel, true);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(int), "ThoriumServer", WellKnownObjectMode.Singleton);
            RemotingServices.Marshal(serverInterface, "ThoriumServer");
        }
    }
}
