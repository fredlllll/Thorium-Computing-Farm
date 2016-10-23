using System;

namespace Codolith.Util
{
    /// <summary>
    /// Scheduled object.
    /// </summary>
    public class ScheduledObject
    {

        public IScheduleType ScheduleType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the argument.
        /// </summary>
        /// <value>The argument.</value>
        public object Arg
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>The function.</value>
        public Action<RescheduleObject, object> Method
        {
            get;
            set;
        }

        /// <summary>
        /// Schedules this in seconds with an argument.
        /// </summary>
        /// <param name="seconds">Seconds.</param>
        /// <param name="arg">Argument.</param>
        /// <param name="scheduler">Scheduler.</param>
        public void Schedule(Action<RescheduleObject, object> method, float seconds, object arg, IScheduler scheduler)
        {
            Method = method;
            ScheduleType = new ScheduleTypeSeconds(seconds, scheduler);
            Arg = arg;
        }

        /// <summary>
        /// Schedules this in ticks with an argument.
        /// </summary>
        /// <param name="ticks">Ticks.</param>
        /// <param name="arg">Argument.</param>
        /// <param name="scheduler">Scheduler.</param>
        public void Schedule(Action<RescheduleObject, object> method, ulong ticks, object arg, IScheduler scheduler)
        {
            Method = method;
            ScheduleType = new ScheduleTypeTicks(ticks, scheduler);
            Arg = arg;
        }

        /// <summary>
        /// Schedules this now with an argument.
        /// </summary>
        /// <param name="arg">Argument.</param>
        public void Schedule(Action<RescheduleObject, object> method, object arg)
        {
            Method = method;
            ScheduleType = new ScheduleTypeNow();
            Arg = arg;
        }
    }
}
