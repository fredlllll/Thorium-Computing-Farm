using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium.Shared.DTOs;

namespace Thorium.Shared.FunctionServer.Tcp
{
    public interface ITcpFunctionProvider
    {
        string FunctionName {  get; }
        object Execute(FunctionServerTcpClient client, object[] args);
    }
}
