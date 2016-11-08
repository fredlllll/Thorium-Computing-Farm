using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
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
        const string configFileName = "serverconfig.xml";

        Config config;
        TcpServerChannel tcpChannel;
        ThoriumClientServerInterface serverInterface;

        public ClientManager ClientManager { get; } = new ClientManager();
        public TaskManager TaskManager { get; } = new TaskManager();
        public JobManager JobManager { get; }

        public ThoriumServer()
        {
            JobManager = new JobManager(TaskManager);

            config = new Config(new FileInfo(configFileName));
            serverInterface = new ThoriumClientServerInterface(this);//does this belong here or in start?
        }

        public void Start()
        {
            int instanceServerPort = config.GetInt("instanceServerPort");

            tcpChannel = new TcpServerChannel(instanceServerPort);
            ChannelServices.RegisterChannel(tcpChannel, true);
            RemotingServices.Marshal(serverInterface, Constants.THORIUM_SERVER_INTERFACE_FOR_CLIENT);
        }

        public void Stop()
        {
            RemotingServices.Disconnect(serverInterface);
            ChannelServices.UnregisterChannel(tcpChannel);
        }
    }
}
