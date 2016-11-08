using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public enum TaskState
    {
        NotStarted,
        Processing,
        Finished
    }

    public abstract class Task : MarshalByRefObject
    {
        public string JobID { get; }
        public string ID { get; }
        public string ProcessingClientID { get; set; }
        public TaskState State { get; set; }

        public Task(string parentJobID)
        {
            JobID = parentJobID;
            ID = Util.GetRandomID();
        }

        public abstract void PopulateJobExecutionInfo(IJobPartExecutionInfo part);
    }
}
