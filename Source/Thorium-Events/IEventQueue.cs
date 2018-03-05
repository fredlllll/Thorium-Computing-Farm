using Newtonsoft.Json.Linq;

namespace Thorium_Events
{
    public delegate void EventReceivedHandler(IEventQueue sender, JToken arg);
    public interface IEventQueue
    {
        EventReceivedHandler EventReceivedHandler { get; set; }

        string Name { get; }

        void AttachReceiver();
        void DetachReceiver();

        void Send(JToken arg);
    }
}
