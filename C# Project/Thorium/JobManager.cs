using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thorium_Shared;

namespace Thorium_Server
{
    public class JobManager
    {
        TaskManager taskManager;
        Dictionary<string, Job> jobs = new Dictionary<string, Job>();
        public JobManager(TaskManager taskManager)
        {
            this.taskManager = taskManager;
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
