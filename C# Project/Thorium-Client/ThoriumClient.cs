using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Client
{
    public class ThoriumClient
    {
        const string configFileName = "clientconfig.xml";

        
        ChannelFactory<IThoriumServerInterfaceForClient> channelFactory;
        IThoriumServerInterfaceForClient ServerInterface { get; set; }
        ThoriumClientInterfaceForServer instance;
        ClientServiceManager clientServiceManager = new ClientServiceManager();
        Config Config { get; set; }
        Thread runner;
        public ThoriumClient()
        {
            Config = new Config(new FileInfo(configFileName));
            SharedData.Set(ClientConfigConstants.SharedDataID_ClientConfig,Config);

            NetTcpBinding tcpBinding = new NetTcpBinding();
            string serverAddress = Config.GetString("serverAddress");
            EndpointAddress wcfAddress = new EndpointAddress("net.tcp://" + serverAddress + "/"+Constants.THORIUM_SERVER_INTERFACE_FOR_CLIENT);
            channelFactory = new ChannelFactory<IThoriumServerInterfaceForClient>(tcpBinding, wcfAddress);
            ServerInterface = channelFactory.CreateChannel();
            

            instance = new ThoriumClientInterfaceForServer();

            ServerInterface.RegisterClient(instance);
            runner = new Thread(Run);

            SharedData.Set(ClientConfigConstants.SharedDataID_ThoriumClient, this);
        }

        public void Start()
        {
            runner.Start();
        }

        public void Shutdown()
        {
            ServerInterface.UnregisterClient(instance);
            ServerInterface = null;
            runner.Interrupt();
            channelFactory.Close();
        }

        void Run()
        {
            DateTime lastTimeJobCompleted = DateTime.UtcNow;
            try
            {
                while(true)
                {
                    var task = ServerInterface?.GetTask(instance);
                    if(task != null)
                    {
                        var execInfo = task.GetExecutionInfo();
                        try
                        {
                            execInfo.Setup();
                            execInfo.Run();
                            execInfo.Cleanup();
                            ServerInterface?.TurnInTask(task);//this can be put in a seperate thread at some point
                        }
                        catch(Exception execEx)
                        {
                            ServerInterface?.ReturnUnfinishedTask(task, execEx.ToString());
                        }
                        lastTimeJobCompleted = DateTime.UtcNow;
                    }
                    else
                    {
                        if((DateTime.UtcNow - lastTimeJobCompleted).TotalSeconds > 180) //if idle for x seconds we shutdown
                        {
                            break;
                        }
                        Thread.Sleep(5000);
                    }
                }
            }
            catch(ThreadInterruptedException)
            {
                //bye bye
            }
            catch(Exception ex)
            {
                //TODO: log
                ServerInterface?.UnregisterClient(instance);
                Util.ShutdownSystem();
            }

        }
    }
}
