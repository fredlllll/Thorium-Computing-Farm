using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public enum JobPartState
    {
        NotStarted,
        Processing,
        Finished
    }

    public abstract class JobPart : MarshalByRefObject
    {
        public string JobID { get; }
        public string ID { get; }
        public string ProcessingClientID { get; set; }
        public JobPartState State { get; set; }

        public JobPart(string parentJobID)
        {
            JobID = parentJobID;
            ID = Util.GetRandomID();
        }

        public abstract void PopulateJobExecutionInfo(IJobPartExecutionInfo part);
    }
}
