using Newtonsoft.Json.Linq;

namespace Thorium_Shared.ServicePoint
{
    public interface IServiceInvoker
    {
        JToken Invoke(string routine, JToken arg);
    }
}
