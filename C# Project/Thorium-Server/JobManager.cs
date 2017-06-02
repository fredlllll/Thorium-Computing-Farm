using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Thorium_Shared;
using Codolith.Config;
using static Thorium_Shared.ConfigKeys.ServerConfigKeys;
using static Thorium_Shared.ConfigKeys.JobConfigKeys;
using static Thorium_Server.ServerStatics;

namespace Thorium_Server
{
    public class JobManager
    {
        JobInitializator jobInitializator = new JobInitializator();
        Dictionary<string, AJob> jobs = new Dictionary<string, AJob>();
        public IEnumerable<KeyValuePair<string, AJob>> Jobs
        {
            get { return jobs; }
        }

        public JobManager()
        {
            jobInitializator.JobInitializationFailed += JobInitializationFailed;
            jobInitializator.JobInitialized += JobInitialized;
        }

        private void JobInitialized(JobInitializator sender, AJob job)
        {
            Logger.LogInfo("Job initialized: " + job.Name);
            jobs[job.ID] = job;
        }

        private void JobInitializationFailed(JobInitializator sender, AJob job, Exception ex)
        {
            Logger.LogWarning("Job initialization failed: " + job.Name, ex.ToString());
        }

        public void Startup()
        {
            jobInitializator.Start();

            //TODO: load serialized jobs
            DirectoryInfo jobsFolder = new DirectoryInfo(ServerStatics.ServerConfig.Get(Key_JobsFolder));
            Directory.CreateDirectory(jobsFolder.FullName);
            var jobs = jobsFolder.GetFiles("*.xml");
            foreach(var job in jobs)
            {
                Config jobConfig = new Config(job.FullName);
                try
                {
                    AJob j = GetNewJob(jobConfig);
                    if(j != null)
                    {
                        AddJob(j);
                    }
                    else
                    {
                        Console.WriteLine("Could not find type for " + job);
                    }
                }
                catch(Exception jobCreateEx)
                {
                    Console.WriteLine("Exception when creating job: " + job);
                    Console.WriteLine(jobCreateEx);
                }
            }
        }

        public void Shutdown()
        {
            //TODO: save stuff
        }

        public void AddJob(AJob job)
        {
            jobInitializator.AddJob(job);
        }

        public void CancelJob(AJob job)
        {
            CancelJob(job.ID);
        }

        public void CancelJob(string id) {
            AJob job;
            if(!jobs.TryGetValue(id, out job)) {
                //TODO: add to cancel list/handler
            }
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

        public AJob GetNewJob(Config config)
        {
            var jobType = config.Get(Key_JobType);
            Type type = Codolith.Reflection.ReflectionHelper.GetTypeByShortName(jobType).FirstOrDefault();
            if(type != null)
            {
                return (AJob)Activator.CreateInstance(type, config);
            }
            return null;
        }
    }
}
