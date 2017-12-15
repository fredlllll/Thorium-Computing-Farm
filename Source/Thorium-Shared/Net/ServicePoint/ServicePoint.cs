using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Net.ServicePoint
{
    public class ServicePoint
    {
        private bool started = false;

        private readonly Dictionary<string, Routine> routines = new Dictionary<string, Routine>();
        private readonly List<IServiceInvokationReceiver> invokers = new List<IServiceInvokationReceiver>();

        //TODO: service point configuration file?

        private void CheckStarted()
        {
            if(started)
            {
                throw new InvalidOperationException("Can't register things after start");
            }
        }

        public void RegisterRoutine(Routine routine)
        {
            CheckStarted();
            if(routines.ContainsKey(routine.Name))
            {
                throw new ArgumentException("There is already a routine named '" + routine.Name + "' registered");
            }
            routines[routine.Name] = routine;
        }

        public void RegisterInvokationReceiver(IServiceInvokationReceiver si)
        {
            invokers.Add(si);
        }

        public void Start()
        {
            if(started)
            {
                throw new InvalidOperationException("Can't start more than once");
            }

            foreach(var si in invokers)
            {
                si.InvokationReceived += HandleInvokationReceived;
                si.Start();
            }
        }

        public void Stop()
        {
            if(!started)
            {
                throw new InvalidOperationException("Can't stop before starting");
            }

            foreach(var si in invokers)
            {
                si.InvokationReceived -= HandleInvokationReceived;
                si.Stop();
            }
        }

        private InvokationResult HandleInvokationReceived(IServiceInvokationReceiver sender, string routine, JToken arg)
        {
            if(routines.TryGetValue(routine, out Routine r))
            {
                try
                {
                    JToken retval = r.Invoke(arg);
                    return new InvokationResult() { ReturnValue = retval };
                }
                catch(Exception ex)
                {
                    return new InvokationResult() { Exception = ex };
                }
            }
            return new InvokationResult() { Exception = new Exception("No routine named '" + routine + "' registered") };
        }
    }
}
