using System;
using System.Collections.Generic;

namespace Codolith.Util
{
    public class SchedulerThreadSafe : IScheduler
    {
        static object instanceLock = new object();
        static SchedulerThreadSafe instance = null;
        public static SchedulerThreadSafe Instance
        {
            get
            {
                if(instance == null)
                {
                    lock(instanceLock)
                    {
                        if(instance == null) //this check is threadsafe, while the outer is just there to ensure not every call will try to get a lock around the if
                        {
                            instance = new SchedulerThreadSafe();
                        }
                    }
                }
                return instance;
            }
        }

        private object scheduledLock = new object();
        private object toSchedLock = new object();
        private object toDelLock = new object();

        /// <summary>
        /// Determines if the Scheduler is currently in the update loop. This is used to determine if scheduled objects can directly be added to the 'scheduled' list or have to be added to the 'toShed' list.
        /// </summary>
        private bool inLoop = false;
        /// <summary>
        /// The list containing all scheduled objects.
        /// </summary>
        private List<ScheduledObject> scheduled = new List<ScheduledObject>();
        /// <summary>
        /// The list containing all objects that should be scheduled after the loop.
        /// </summary>
        private List<ScheduledObject> toSched = new List<ScheduledObject>();
        /// <summary>
        /// The list containing all objects that should be unscheduled after the loop.
        /// </summary>
        private List<ScheduledObject> toDel = new List<ScheduledObject>();

        /// <summary>
        /// The time that has been elapsed since the initialization of the scheduler
        /// </summary>
        /// <value>The elapsed time.</value>
        public TimeSpan ElapsedTime
        {
            get;
            private set;
        }

        /// <summary>
        /// The amount of elapsed ticks since the initialization of the scheduler (1 tick is one call to the Update() method)
        /// </summary>
        /// <value>The elapsed ticks.</value>
        public ulong ElapsedTicks
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scheduler"/>.
        /// </summary>
        public SchedulerThreadSafe()
        {
            ElapsedTime = new TimeSpan();
            ElapsedTicks = 0;
        }

        /// <summary>
        /// Schedule the specified method in seconds from now. you can optionally provide an argument that is passed to the function.
        /// It returns a ScheduledObject which can be used to unschedule this method externally.
        /// </summary>
        /// <param name="function">Function.</param>
        /// <param name="seconds">Seconds.</param>
        /// <param name="arg">Argument.</param>
        public ScheduledObject Schedule(Action<RescheduleObject, object> method, float seconds, object arg)
        {
            ScheduledObject so = new ScheduledObject();
            so.Schedule(method, seconds, arg, this);
            if(inLoop)
            {
                lock(toSchedLock)
                {
                    toSched.Add(so);
                }
            }
            else
            {
                lock(scheduledLock)
                {
                    scheduled.Add(so);
                }
            }
            return so;
        }

        /// <summary>
        /// Schedule the specified method in ticks from now. you can optionally provide an argument that is passed to the function.
        /// It returns a ScheduledObject which can be used to unschedule this method externally.
        /// </summary>
        /// <param name="function">Function.</param>
        /// <param name="ticks">Ticks.</param>
        /// <param name="arg">Argument.</param>
        public ScheduledObject Schedule(Action<RescheduleObject, object> method, ulong ticks, object arg)
        {
            ScheduledObject so = new ScheduledObject();
            so.Schedule(method, ticks, arg, this);
            if(inLoop)
            {
                lock(toSchedLock)
                {
                    toSched.Add(so);
                }
            }
            else
            {
                lock(scheduledLock)
                {
                    scheduled.Add(so);
                }
            }
            return so;
        }

        /// <summary>
        /// Schedule the specified method now. you can optionally provide an argument that is passed to the function.
        /// It returns a ScheduledObject which can be used to unschedule this method externally.
        /// </summary>
        /// <param name="function">Function.</param>
        /// <param name="arg">Argument.</param>
        public ScheduledObject Schedule(Action<RescheduleObject, object> method, object arg)
        {
            ScheduledObject so = new ScheduledObject();
            so.Schedule(method, arg);
            if(inLoop)
            {
                lock(toSchedLock)
                {
                    toSched.Add(so);
                }
            }
            else
            {
                lock(scheduledLock)
                {
                    scheduled.Add(so);
                }
            }
            return so;
        }

        /// <summary>
        /// Unschedule the specified ScheduledObject.
        /// </summary>
        /// <param name="so">ScheduledObject that u got when scheduling the method</param>
        public void Unschedule(ScheduledObject so)
        {
            if(inLoop)
            {
                lock(toDelLock)
                {
                    toDel.Add(so);
                }
            }
            else
            {
                lock(scheduledLock)
                {
                    scheduled.Remove(so);
                }
            }
        }

        /// <summary>
        /// Increases elapsed ticks and time. Runs scheduled methods if they are due. Processes toSched and toDel.
        /// elapsed is measured in seconds
        /// </summary>
        public void UpdateFunction(float elapsed)
        {
            ElapsedTime = ElapsedTime.Add(new TimeSpan((long)(TimeSpan.TicksPerSecond * elapsed)));
            ElapsedTicks++;

            RescheduleObject ro = new RescheduleObject(this);
            inLoop = true; //so you can schedule stuff inside the methods
            lock(scheduledLock)
            {
                foreach(ScheduledObject so in scheduled)
                {
                    if(so.ScheduleType.IsDue(this))
                    {
                        ro.Reset(so);
                        so.Method(ro, so.Arg);
                        switch(ro.RescheduleType)
                        {
                            case RescheduleType.Reschedule:
                                so.Arg = ro.Arg;
                                so.Method = ro.Method;
                                so.ScheduleType = ro.ScheduleType;
                                break;
                            case RescheduleType.Unschedule:
                                lock(toDelLock)
                                {
                                    toDel.Add(so);
                                }
                                break;
                        }
                    }

                }
            }
            inLoop = false;
            lock(toSchedLock)
            {
                lock(scheduledLock)
                {
                    foreach(ScheduledObject so in toSched)
                    {
                        scheduled.Add(so);
                    }
                }
                toSched.Clear();
            }
            lock(toDelLock)
            {
                lock(scheduledLock)
                {
                    foreach(ScheduledObject so in toDel)
                    {
                        scheduled.Remove(so);
                    }
                }
                toDel.Clear();
            }
        }
    }
}
