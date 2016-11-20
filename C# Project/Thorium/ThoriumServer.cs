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

        Config config;
        TcpServerChannel tcpChannel;
        ThoriumClientServerInterface serverInterface;

        public ClientManager ClientManager { get; } = new ClientManager();
        public TaskManager TaskManager { get; } = new TaskManager();
        public JobManager JobManager { get; }
        //public DBUtil DBUtil { get; }

        public ThoriumServer()
        {
            JobManager = new JobManager(TaskManager);
            TaskManager.JobManager = JobManager;

            config = new Config(new FileInfo(configFileName));
            SharedData.Set(ServerConfigConstants.sharedServerConfigName, this.config);
            serverInterface = new ThoriumClientServerInterface(this);
        }

        public void Start()
        {
            int instanceServerPort = config.GetInt(ServerConfigConstants.remotingServerPort);

            tcpChannel = new TcpServerChannel(instanceServerPort);
            ChannelServices.RegisterChannel(tcpChannel, true);
            RemotingServices.Marshal(serverInterface, Constants.THORIUM_SERVER_INTERFACE_FOR_CLIENT);

            DirectoryInfo jobsFolder = new DirectoryInfo(config.GetString(ServerConfigConstants.jobsFolder));
            var jobs = jobsFolder.GetFiles("*.xml");
            foreach(var job in jobs)
            {
                Config jobConfig = new Config(job);
                try
                {
                    Job j = JobManager.GetNewJob(jobConfig);
                    if(j != null)
                    {
                        JobManager.AddJob(j);
                    }
                    else
                    {
                        Console.WriteLine("Could not find type for " + job);
                    }
                }
                catch(Exception jobCreateEx)
                {
                    Console.WriteLine("Exception when creating job: " + job);
                    Console.WriteLine(jobCreateEx);
                }
            }
        }

        public void Stop()
        {
            RemotingServices.Disconnect(serverInterface);
            ChannelServices.UnregisterChannel(tcpChannel);
        }
    }
}
