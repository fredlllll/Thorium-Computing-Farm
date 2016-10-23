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
    public abstract class Job
    {
        public string ID { get; set; }
        public ThoriumServer server;
        public string Name { get; set; }
        public List<FrameBounds> Frames { get; } = new List<FrameBounds>();
        public BackendConfig Config { get; set; }
        ConcurrentDictionary<string, SubJob> unfinishedSubJobs = new ConcurrentDictionary<string, SubJob>();
        ConcurrentBag<SubJob> finishedJobs = new ConcurrentBag<SubJob>();

        public Job(ThoriumServer server) {
            this.server = server;
        }

        public SubJob GetSubJob()
        {
            SubJob sj = new SubJob(ID);
            Config.PopulateSubJob(sj);
            unfinishedSubJobs[sj.ID] = sj;
            return sj;
        }

        public void FinishSubJob(SubJob job)
        {
            if(unfinishedSubJobs.TryRemove(job.ID, out job))
            {
                finishedJobs.Add(job);
            }
            //check if job is finished
            if(false)//TODO: create condition to 
            {
                Job j;
                if(server.Jobs.TryRemove(ID, out j))
                {
                    server.FinishedJobs.Enqueue(j);
                }
            }
        }
    }
}
