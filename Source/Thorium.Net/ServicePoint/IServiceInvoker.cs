using Newtonsoft.Json.Linq;

namespace Thorium_Net
{
    public interface IServiceInvoker
    {
        JToken Invoke(string routine, JToken arg);
    }
}
