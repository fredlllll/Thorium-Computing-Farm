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
using Codolith.DBUtil;
using Thorium_Shared;

namespace Thorium_Server
{
    public class ThoriumServer
    {
        const string configFileName = "serverconfig.xml";

        public Config Config { get; private set; }
        TcpServerChannel tcpChannel;
        ThoriumClientServerInterface serverInterface;

        public ClientManager ClientManager { get; } = new ClientManager();
        public TaskManager TaskManager { get; } = new TaskManager();
        public JobManager JobManager { get; }
        public ServerServiceManager ServerServiceManager { get; } = new ServerServiceManager();
        //public DBUtil DBUtil { get; }

        public ThoriumServer()
        {
            Config = new Config(new FileInfo(configFileName));
            JobManager = new JobManager(TaskManager, this);
            TaskManager.JobManager = JobManager;

            SharedData.Set(ServerConfigConstants.SharedDataID_ServerConfig, this.Config);
            serverInterface = new ThoriumClientServerInterface(this);
        }

        public void Start()
        {
            int instanceServerPort = Config.GetInt(ServerConfigConstants.remotingServerPort);

            tcpChannel = new TcpServerChannel(instanceServerPort);
            ChannelServices.RegisterChannel(tcpChannel, true);
            RemotingServices.Marshal(serverInterface, Constants.THORIUM_SERVER_INTERFACE_FOR_CLIENT);

            JobManager.Initialize();
        }

        public void Stop()
        {
            RemotingServices.Disconnect(serverInterface);
            ChannelServices.UnregisterChannel(tcpChannel);
        }
    }
}
