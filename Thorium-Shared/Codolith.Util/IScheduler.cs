using System;

namespace Codolith.Util
{
    public interface IScheduler
    {
        TimeSpan ElapsedTime
        {
            get;
        }

        ulong ElapsedTicks
        {
            get;
        }

        ScheduledObject Schedule(Action<RescheduleObject, object> method, float seconds, object arg);
        ScheduledObject Schedule(Action<RescheduleObject, object> method, ulong ticks, object arg);
        ScheduledObject Schedule(Action<RescheduleObject, object> method, object arg);

        void Unschedule(ScheduledObject so);

        void UpdateFunction(float elapsed);
    }
}
