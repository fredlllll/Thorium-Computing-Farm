using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public abstract class ATaskProducer
    {
        public Job Job { get; protected set; }

        public ATaskProducer(Job job)
        {
            Job = job;
        }

        //public abstract Task GetNextTask();
        public abstract IEnumerator<Task> GetTasks();
    }
}
