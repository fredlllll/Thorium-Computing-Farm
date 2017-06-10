using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Jobtypes.SimpleExecution
{
    public class SETaskInformationProducer : ATaskInformationProducer
    {
        Queue<TaskInformation> taskInfos = new Queue<TaskInformation>();
        Dictionary<string, TaskInformation> processing = new Dictionary<string, TaskInformation>();

        public SETaskInformationProducer(AJob job, JobInformation jobInformation) : base(job)
        {
            int cnt = jobInformation.Config.Get<int>("count");

            for(int i = 0; i < cnt; i++)
            {
                TaskInformation ti = new TaskInformation();
                ti.JobID = job.ID;
                ti.ID = Utils.GetRandomID();
                ti.TaskType = typeof(SETask);
                ti.Config.Set("index", i);
                ti.Config.Set("program", jobInformation.Config.Get("program"));
                taskInfos.Enqueue(ti);
            }
        }

        public override int RemainingTaskInformationCount
        {
            get
            {
                return taskInfos.Count;
            }
        }

        public override TaskInformation GetNextTaskInformation()
        {
            var ti = taskInfos.Dequeue();
            processing[ti.ID] = ti;
            return ti;
        }

        public override void SignalTaskAborted(string id, string reason)
        {
            var ti = processing[id];
            processing.Remove(id);
            taskInfos.Enqueue(ti);
        }

        public override void SignalTaskFinished(string id)
        {
            processing.Remove(id);
        }
    }
}
