using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Thorium_Shared;

namespace Thorium_Server
{
    public class JobManager
    {
        TaskManager taskManager;
        ThoriumServer server;
        Dictionary<string, Job> jobs = new Dictionary<string, Job>();
        public JobManager(TaskManager taskManager,ThoriumServer server)
        {
            this.taskManager = taskManager;
            this.server = server;
        }

        public void Initialize()
        {
            DirectoryInfo jobsFolder = new DirectoryInfo(server.Config.GetString(ServerConfigConstants.jobsFolder));
            Directory.CreateDirectory(jobsFolder.FullName);
            var jobs = jobsFolder.GetFiles("*.xml");
            foreach(var job in jobs)
            {
                Config jobConfig = new Config(job);
                try
                {
                    Job j = GetNewJob(jobConfig);
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

        public void AddJob(Job job)
        {
            job.InitializeTasks();
            foreach(var t in job.Tasks)
            {
                taskManager.RegisterTask(t);
            }
            jobs[job.ID] = job;
        }

        public void RemoveJob(Job job)
        {
            foreach(var t in job.Tasks)
            {
                taskManager.UnregisterTask(t);
            }
            jobs.Remove(job.ID);
        }

        public Job GetJobById(string jobID)
        {
            Job j;
            if(jobs.TryGetValue(jobID, out j))
            {
                return j;
            }
            return null;
        }

        public Job GetNewJob(Config config)
        {
            var jobType = config.GetString(JobConfigConstants.jobType);
            Type type = Codolith.Reflection.ReflectionHelper.GetTypeByShortName(jobType).FirstOrDefault();
            if(type != null)
            {
                return (Job)Activator.CreateInstance(type, config);
            }
            return null;
        }
    }
}
