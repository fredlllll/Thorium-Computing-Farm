using System;
using System.Net;
using System.Net.Sockets;

namespace Thorium.Test
{
    public class Program
    {
        static UdpClient recv;

        public static void Main()
        {
            recv = new UdpClient(8300);
            UdpClient send = new UdpClient();

            recv.BeginReceive(Receive, null);

            IPEndPoint ep = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 8300);

            //send.
            send.Send(new byte[] { 42, 42, 42 }, 3, ep);

            Console.ReadKey();
        }

        static void Receive(IAsyncResult result)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 8300);

            var bytes = recv.EndReceive(result, ref ep);
            byte b = bytes[0];
        }
    }
}
