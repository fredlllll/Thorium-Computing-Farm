using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Client
{
    public class ThoriumClient
    {
        TcpClientChannel tcpChannel;
        IThoriumServerInterfaceForClient serverInterface;
        Instance instance;
        Thread runner;
        public ThoriumClient()
        {
            tcpChannel = new TcpClientChannel();
            ChannelServices.RegisterChannel(tcpChannel, true);
            serverInterface = (IThoriumServerInterfaceForClient)Activator.GetObject(typeof(IThoriumServerInterfaceForClient), "tcp://127.0.0.1/"+ Constants.THORIUM_SERVER_INTERFACE_FOR_CLIENT);
            instance = new Instance();
            if(serverInterface.RegisterInstance(instance))
            {
                runner = new Thread(Run);
            }
            else
            {
                ClientUtil.ShutdownSystem();
            }
        }

        void Run()
        {
            DateTime lastTimeJobCompleted = DateTime.UtcNow;
            try
            {
                //try
                //{
                while(true)
                {
                    var job = serverInterface.GetSubJob();
                    if(job != null)
                    {
                        var je = new JobExecutionInfo(job);
                        je.Execute();
                        serverInterface.FinishSubJob(job);
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
                /*}
                catch(ThreadInterruptedException)
                {

                }*/
            }
            catch(Exception ex)
            {
                //TODO: log
            }
            serverInterface.UnregisterInstance(instance);
            ClientUtil.ShutdownSystem();
        }
    }
}
