using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Net.ServicePoint
{
    public interface IServiceInvoker
    {
        JToken Invoke(string routine, JToken arg);
    }
}
