using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codolith.Util;
using Thorium_Shared;

namespace Thorium_Server
{
    public class ClientInstanceRequest
    {
        ClientManager manager;
        ScheduledObject schedObj;

        public ClientInstanceRequest(ClientManager manager)
        {
            this.manager = manager;
            schedObj = SchedulerThreadSafe.Instance.Schedule(this.Timeout, 120f, null);
            StartInstance();
        }

        void Timeout(RescheduleObject ro, object arg) {
            ro.Reschedule(120f);
            StartInstance();
        }

        void StartInstance()
        {
            //TODO: start instance
        }

        public void Satisfy()
        {
            SchedulerThreadSafe.Instance.Unschedule(schedObj);
        }
    }
}
