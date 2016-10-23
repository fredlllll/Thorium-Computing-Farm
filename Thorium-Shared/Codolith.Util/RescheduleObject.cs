using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codolith.Util
{
    /// <summary>
    /// Type of rescheduling. Either InSeconds,InTicks,Repeat or Unschedule
    /// </summary>
    public enum RescheduleType
    {
        Reschedule,
        Unschedule
    }

    public class RescheduleObject
    {

        public IScheduler Scheduler
        {
            get;
            private set;
        }

        /// <summary>
        /// The reschedule type.
        /// </summary>
        /// <value>The type of the reschedule.</value>
        public RescheduleType RescheduleType
        {
            get;
            set;
        }

        public IScheduleType ScheduleType
        {
            get;
            set;
        }

        /// <summary>
        /// The argument.
        /// </summary>
        /// <value>The argument.</value>
        public object Arg
        {
            get;
            set;
        }


        public Action<RescheduleObject, object> Method
        {
            get;
            set;
        }

        public RescheduleObject(IScheduler scheduler)
        {
            Scheduler = scheduler;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset(ScheduledObject so)
        {
            RescheduleType = RescheduleType.Reschedule;
            if(so != null)
            {
                Arg = so.Arg;
                Method = so.Method;
                ScheduleType = so.ScheduleType;
            }
        }

        public void Reschedule(ulong ticks)
        {
            Reschedule(ticks, Arg);
        }

        public void Reschedule(ulong ticks, object arg)
        {
            Reschedule(ticks, arg, Method);
        }

        public void Reschedule(ulong ticks, object arg, Action<RescheduleObject, object> method)
        {
            Reschedule(new ScheduleTypeTicks(ticks, Scheduler), arg, method);
        }

        public void Reschedule(float seconds)
        {
            Reschedule(seconds, Arg);
        }

        public void Reschedule(float seconds, object arg)
        {
            Reschedule(seconds, arg, Method);
        }

        public void Reschedule(float seconds, object arg, Action<RescheduleObject, object> method)
        {
            Reschedule(new ScheduleTypeSeconds(seconds, Scheduler), arg, method);
        }

        public void Reschedule(IScheduleType scheduleType, object arg, Action<RescheduleObject, object> method)
        {
            ScheduleType = scheduleType;
            Arg = arg;
            Method = method;
        }

        public void Unschedule()
        {
            RescheduleType = RescheduleType.Unschedule;
        }
    }
}
