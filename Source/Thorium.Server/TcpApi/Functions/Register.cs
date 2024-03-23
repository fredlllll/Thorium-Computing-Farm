using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium.Shared.FunctionServer.Tcp;

namespace Thorium.Server.TcpApi.Functions
{
    public class Register : ITcpFunctionProvider
    {
        public string FunctionName => "register";

        public object Execute(FunctionServerTcpClient client, object[] args)
        {
            Register_(client, (string)args[0]);
            return null;
        }

        void Register_(FunctionServerTcpClient client, string id)
        {
            client.Name = id;
        }
    }
}
