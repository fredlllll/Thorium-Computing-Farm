using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thorium_Server
{
    public class JobManager
    {
        TaskManager taskManager;
        public JobManager(TaskManager taskManager)
        {
            this.taskManager = taskManager;
        }

        public void AddJob(Job job)
        {

        }

        public void RemoveJob(Job job)
        {

        }
    }
}
