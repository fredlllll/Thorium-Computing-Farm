using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium.Shared.FunctionServer.Tcp;

namespace Thorium.Server.TcpApi.Functions
{
    public class Heartbeat : ITcpFunctionProvider
    {
        public string FunctionName => "heartbeat";

        public object Execute(FunctionServerTcpClient client, object[] args)
        {
            Heartbeat_(client);
            return null;
        }

        void Heartbeat_(FunctionServerTcpClient client)
        {
            //TODO
        }
    }
}
