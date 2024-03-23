using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Thorium.Shared.FunctionServer.Http
{
    public interface IHttpFunctionProvider
    {
        string FunctionName { get; }
        void Execute(HttpListenerContext context);
    }
}
