using System.Net.Sockets;

namespace Thorium_Shared.Net.Comms
{
    public static class CommsFactory
    {
        /// <summary>
        /// returns an unstarted ServiceServer that will listen on the given port
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static ServiceServer CreateServer(ushort port)
        {
            ServiceServer server = new ServiceServer(typeof(JsonTransceiver), port);
            return server;
        }

        /// <summary>
        /// returns a ServiceClient that you can invoke commands on
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static ServiceClient CreateClient(string host, ushort port)
        {
            var client = new TcpClient();
            client.Connect(host, port);

            var transceiver = new JsonTransceiver(client);

            ServiceClient serviceClient = new ServiceClient(transceiver);
            return serviceClient;
        }
    }
}
