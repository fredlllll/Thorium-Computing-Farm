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
        ThoriumClientInterfaceForServer instance;
        Thread runner;
        public ThoriumClient()
        {
            tcpChannel = new TcpClientChannel();
            ChannelServices.RegisterChannel(tcpChannel, true);
            serverInterface = (IThoriumServerInterfaceForClient)Activator.GetObject(typeof(IThoriumServerInterfaceForClient), "tcp://127.0.0.1/" + Constants.THORIUM_SERVER_INTERFACE_FOR_CLIENT);
            instance = new ThoriumClientInterfaceForServer();

            serverInterface.RegisterClient(instance);
            runner = new Thread(Run);
            /*if(serverInterface.RegisterClient(instance))
            {
                runner = new Thread(Run);
            }
            else
            {
                Shutdown();
                Util.ShutdownSystem();
            }*/
        }

        public void Shutdown()
        {
            serverInterface.UnregisterClient(instance);
            serverInterface = null;
            runner.Interrupt();
            ChannelServices.UnregisterChannel(tcpChannel);

        }

        void Run()
        {
            DateTime lastTimeJobCompleted = DateTime.UtcNow;
            try
            {
                while(true)
                {
                    var task = serverInterface?.GetTask(instance);
                    if(task != null)
                    {
                        var execInfo = task.GetExecutionInfo();
                        try
                        {
                            execInfo.Setup();
                            execInfo.Run();
                            serverInterface?.TurnInTask(task,execInfo.GetResultZip());
                        }
                        catch(Exception execEx)
                        {
                            serverInterface.ReturnUnfinishedTask(task, execEx.ToString());
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
                serverInterface?.UnregisterClient(instance);
                Util.ShutdownSystem();
            }

        }
    }
}
