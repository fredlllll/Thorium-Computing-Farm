using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codolith.Util
{
    public interface IScheduleType
    {
        bool IsDue(IScheduler scheduler);
    }

    public class ScheduleTypeSeconds : IScheduleType
    {
        private TimeSpan startTime;
        public float Seconds
        {
            get;
            private set;
        }

        public ScheduleTypeSeconds(float seconds, IScheduler scheduler)
        {
            Seconds = seconds;
            startTime = scheduler.ElapsedTime.Add(new TimeSpan((long)(TimeSpan.TicksPerSecond * seconds)));
        }

        public bool IsDue(IScheduler scheduler)
        {
            return scheduler.ElapsedTime > startTime;
        }
    }

    public class ScheduleTypeTicks : IScheduleType
    {
        private ulong startTicks;
        public ulong Ticks
        {
            get;
            private set;
        }

        public ScheduleTypeTicks(ulong ticks, IScheduler scheduler)
        {
            Ticks = ticks;
            startTicks = scheduler.ElapsedTicks + ticks;
        }

        public bool IsDue(IScheduler scheduler)
        {
            return scheduler.ElapsedTicks > startTicks;
        }
    }

    public class ScheduleTypeNow : IScheduleType
    {
        public bool IsDue(IScheduler scheduler)
        {
            return true;
        }
    }
}
