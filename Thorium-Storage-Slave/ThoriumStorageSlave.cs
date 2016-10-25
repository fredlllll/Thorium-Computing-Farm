using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Storage_Slave
{
    public class ThoriumStorageSlave
    {
        TcpClientChannel tcpChannel;
        IThoriumServerInterfaceForStorage serverInterface;
        IThoriumStorageInterfaceForServer storageInterface = new ThoriumStorageInterfaceForServer();

        public ThoriumStorageSlave()
        {
            tcpChannel = new TcpClientChannel();
            ChannelServices.RegisterChannel(tcpChannel,true);
            serverInterface = (IThoriumServerInterfaceForStorage)Activator.GetObject(typeof(IThoriumServerInterfaceForStorage), "tcp://127.0.0.1/"+Constants.THORIUM_SERVER_INTERFACE_FOR_STORAGE);
            if(serverInterface.RegisterInstance(storageInterface))
            {
                var servers = serverInterface.GetStorageServerAddressList();
                var files = serverInterface.GetFileList();
                //start a thread to copy files?
            }
            else
            {
                Util.ShutdownSystem();
            }
        }

        public void Shutdown()
        {
            serverInterface.UnregisterInstance(storageInterface);
            ChannelServices.UnregisterChannel(tcpChannel);
        }
    }
}
