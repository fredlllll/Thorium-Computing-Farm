using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Net.ServicePoint
{
    public delegate InvokationResult InvokationHandler(IServiceInvokationReceiver sender, string routine, JToken arg);

    public interface IServiceInvokationReceiver
    {
        event InvokationHandler InvokationReceived;

        void Start();
        void Stop();
    }
}
