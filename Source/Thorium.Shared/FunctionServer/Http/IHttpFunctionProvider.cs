using System.Net;

namespace Thorium.Shared.FunctionServer.Http
{
    public interface IHttpFunctionProvider
    {
        string FunctionName { get; }
        void Execute(HttpListenerContext context);
    }
}
