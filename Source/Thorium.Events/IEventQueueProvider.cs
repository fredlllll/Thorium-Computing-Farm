namespace Thorium_Events
{
    public interface IEventQueueProvider
    {
        IEventQueue GetQueue(string name);
    }
}
