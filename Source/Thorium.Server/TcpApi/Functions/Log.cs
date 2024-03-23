using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium.Shared.FunctionServer.Tcp;

namespace Thorium.Server.TcpApi.Functions
{
    public class Log : ITcpFunctionProvider
    {
        public string FunctionName => "log";

        public object Execute(FunctionServerTcpClient client, object[] args)
        {
            Log_(client, (string)args[0]);
            return null;
        }

        void Log_(FunctionServerTcpClient client, string msg)
        {
            //TODO
        }
    }
}
