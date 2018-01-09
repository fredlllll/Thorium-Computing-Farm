namespace Thorium_Shared.Events
{
    public interface IEventQueueProvider
    {
        IEventQueue GetQueue(string name);
    }
}
