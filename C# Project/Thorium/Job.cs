using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Server
{
    public class Job
    {
        public ThoriumServer server;

        public string ID { get; set; }
        public string Name { get; set; }
        public List<FrameBounds> Frames { get; } = new List<FrameBounds>();

        public BackendConfig Config { get; set; }

        public int TotalJobPartsCount
        {
            get { return Jobs.Count; }
        }
        public int FinishedJobsCount
        {
            get { return FinishedJobs.Count; }
        }
        public int ProcessingJobsCount
        {
            get { return ProcessingJobs.Count; }
        }
        ConcurrentDictionary<string, JobPart> Jobs { get; } = new ConcurrentDictionary<string, JobPart>();
        ConcurrentBag<JobPart> NotStartedJobs { get; } = new ConcurrentBag<JobPart>();
        ConcurrentDictionary<string, JobPart> ProcessingJobs { get; } = new ConcurrentDictionary<string, JobPart>();
        ConcurrentDictionary<string, JobPart> FinishedJobs { get; } = new ConcurrentDictionary<string, JobPart>();

        public Job(ThoriumServer server)
        {
            this.server = server;
            ID = Util.GetRandomID();
        }

        public void InitializeJobs()
        {
            foreach(var j in Config.GetAllJobs(this))
            {
                Jobs[j.ID] = j;
            }
        }

        public void Reset()
        {
            FinishedJobs.Clear();

            JobPart tmp;
            while(NotStartedJobs.TryTake(out tmp))
            { }

            foreach(var kv in ProcessingJobs)
            {
                server.ClientManager.GetClient(kv.Value.ProcessingClientID).AbortJobPart(kv.Value.ID);
            }

            foreach(var kv in Jobs)
            {
                kv.Value.State = JobPartState.NotStarted;
                kv.Value.ProcessingClientID = null;
                NotStartedJobs.Add(kv.Value);
            }

        }

        public JobPart GetNextFreeSubJob(IThoriumClientInterfaceForServer client)
        {
            JobPart j;
            if(NotStartedJobs.TryTake(out j))
            {
                j.State = JobPartState.Processing;
                j.ProcessingClientID = client.ID;
                ProcessingJobs[j.ID] = j;
            }
            return null;
        }

        public void FinishSubJob(JobPart job)
        {
            if(ProcessingJobs.TryRemove(job.ID, out job))
            {
                job.State = JobPartState.Finished;
                FinishedJobs[job.ID] = job;
            }
        }
    }
}
