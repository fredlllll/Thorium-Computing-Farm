using Newtonsoft.Json.Linq;

namespace Thorium_Net
{
    public delegate InvokationResult InvokationHandler(IServiceInvokationReceiver sender, string routine, JToken arg);

    public interface IServiceInvokationReceiver
    {
        event InvokationHandler InvokationReceived;

        void Start();
        void Stop();
    }
}
