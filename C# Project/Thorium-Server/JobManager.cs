using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Thorium_Shared;
using Codolith.Config;
using static Thorium_Shared.ConfigKeys.ServerConfigKeys;
using static Thorium_Shared.ConfigKeys.JobConfigKeys;
using static Thorium_Shared.SharedStatics;
using System.Collections.Concurrent;
using System.Runtime.Serialization;
using Codolith.Serialization;
using Codolith.Serialization.Formatters;
using Thorium_Shared.WCFInterfaces;

namespace Thorium_Server
{
    public class JobManager
    {
        JobInitializer jobInitializer = new JobInitializer();
        Dictionary<string, AJob> jobs = new Dictionary<string, AJob>();
        public IEnumerable<KeyValuePair<string, AJob>> Jobs
        {
            get { return jobs; }
        }

        public JobManager()
        {
            jobInitializer.JobInitializationFailed += JobInitializationFailed;
            jobInitializer.JobInitialized += JobInitialized;
        }

        private void JobInitialized(JobInitializer sender, AJob job)
        {
            Logger.LogInfo("Job initialized: " + job.Name);
            jobs[job.ID] = job;
        }

        private void JobInitializationFailed(JobInitializer sender, AJob job, Exception ex)
        {
            Logger.LogWarning("Job initialization failed: " + job.Name, ex.ToString());
        }

        public void Start()
        {
            jobInitializer.Start();

            //loading jobs
            /*if(File.Exists("jobs.xml"))
            {
                ReferencingSerializer rs = new ReferencingSerializer();
                using(FileStream fs = new FileStream("jobs.xml", FileMode.Open))
                {
                    var formatter = new XMLFormatter(fs);
                    rs.ReadSerializationDataSet(formatter.Read());
                }

                List<JobInformation> jobInfos = new List<JobInformation>();
                foreach(object obj in rs.Objects)
                {
                    if(obj.GetType() == typeof(List<JobInformation>))
                    {
                        jobInfos = (List<JobInformation>)obj;
                        break;
                    }
                }

                foreach(var ji in jobInfos)
                {
                    var j = AJob.JobFromInformation(ji);
                    jobInitializator.AddJob(j);
                }
            }*/
            //TODO: use database
        }

        public void Shutdown()
        {
            //TODO: use database
            /*ReferencingSerializer rs = new ReferencingSerializer();
            using(FileStream fs = new FileStream("jobs.xml", FileMode.Create))
            {
                foreach(var kv in jobs)
                {
                    rs.AddObject(kv.Value);
                }
                var formatter = new XMLFormatter(fs);
                formatter.Write(rs.GetSerializationDataSet());
            }*/
            jobInitializer.Stop();
        }

        public void AddJob(AJob job)
        {
            jobInitializer.AddJob(job);
        }

        public void CancelJob(AJob job)
        {
            //job.Cancel();
            //TODO:????
        }

        /*public void CancelJob(string id)
        {
            CancelJob(GetJobById(id));
        }

        public AJob GetJobById(string jobID)
        {
            AJob j;
            if(jobs.TryGetValue(jobID, out j))
            {
                return j;
            }
            return null;
        }
        */
        
        /*public TaskInformation GetFreeTask(IThoriumClientInterfaceForServer client)
        {
            TaskInformation task = default(TaskInformation);
            foreach(var kv in jobs)
            {
                if((task = kv.Value.GetFreeTask(client)) != default(TaskInformation))
                {
                    break;
                }
            }
            return task;
        }

        public void SignalTaskFinished(string jobID, string id)
        {
            AJob job = default(AJob);
            if(jobs.TryGetValue(jobID, out job))
            {
                job.SignalTaskFinished(id);
            }
        }

        public void SignalTaskAborted(string jobID, string id, string reason)
        {
            AJob job = default(AJob);
            if(jobs.TryGetValue(jobID, out job))
            {
                job.SignalTaskAborted(id, reason);
            }
        }
        */

    }
}
