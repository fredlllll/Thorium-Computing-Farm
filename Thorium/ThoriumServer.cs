using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Server
{
    public class ThoriumServer
    {
        TcpServerChannel tcpChannel;
        ThoriumClientServerInterface serverInterface;

        public InstanceManager InstanceManager { get; } = new InstanceManager();
        public ConcurrentQueue<Job> FinishedJobs { get; } = new ConcurrentQueue<Job>();
        public ConcurrentDictionary<string, Job> Jobs { get; } = new ConcurrentDictionary<string, Job>();

        public ThoriumServer(int instanceServerPort = 8100)
        {
            serverInterface = new ThoriumClientServerInterface(this);
            tcpChannel = new TcpServerChannel(instanceServerPort);
            ChannelServices.RegisterChannel(tcpChannel, true);
            RemotingServices.Marshal(serverInterface, "ThoriumServer");
        }

        public SubJob GetSubJob()
        {
            foreach(var kv in Jobs)
            {
                SubJob sj = kv.Value.GetSubJob();
                if(sj != null)
                {
                    return sj;
                }
            }
            return null;
        }
    }
}
