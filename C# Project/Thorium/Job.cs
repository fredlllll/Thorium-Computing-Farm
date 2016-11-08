using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        ConcurrentDictionary<string, Thorium_Shared.Task> Jobs { get; } = new ConcurrentDictionary<string, Thorium_Shared.Task>();
        ConcurrentBag<Thorium_Shared.Task> NotStartedJobs { get; } = new ConcurrentBag<Thorium_Shared.Task>();
        ConcurrentDictionary<string, Thorium_Shared.Task> ProcessingJobs { get; } = new ConcurrentDictionary<string, Thorium_Shared.Task>();
        ConcurrentDictionary<string, Thorium_Shared.Task> FinishedJobs { get; } = new ConcurrentDictionary<string, Thorium_Shared.Task>();

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

            Thorium_Shared.Task tmp;
            while(NotStartedJobs.TryTake(out tmp))
            { }

            foreach(var kv in ProcessingJobs)
            {
                server.ClientManager.GetClient(kv.Value.ProcessingClientID).AbortJobPart(kv.Value.ID);
            }

            foreach(var kv in Jobs)
            {
                kv.Value.State = TaskState.NotStarted;
                kv.Value.ProcessingClientID = null;
                NotStartedJobs.Add(kv.Value);
            }

        }

        public Task GetNextFreeSubJob(IThoriumClientInterfaceForServer client)
        {
            Thorium_Shared.Task j;
            if(NotStartedJobs.TryTake(out j))
            {
                j.State = TaskState.Processing;
                j.ProcessingClientID = client.ID;
                ProcessingJobs[j.ID] = j;
            }
            return null;
        }

        public void FinishSubJob(Thorium_Shared.Task job)
        {
            if(ProcessingJobs.TryRemove(job.ID, out job))
            {
                job.State = TaskState.Finished;
                FinishedJobs[job.ID] = job;
            }
        }
    }
}
