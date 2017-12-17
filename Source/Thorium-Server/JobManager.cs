using System;
using System.Collections.Generic;
using Thorium_Shared;
using NLog;
using Thorium_Shared.Data.Serializers;

namespace Thorium_Server
{
    public class JobManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly JobInitializer jobInitializer;
        Dictionary<string, Job> jobs = new Dictionary<string, Job>();
        private readonly JobSerializer serializer;

        public IEnumerable<KeyValuePair<string, Job>> Jobs
        {
            get { return jobs; }
        }

        public JobManager(ThoriumServer server, JobSerializer serializer)
        {
            jobInitializer = new JobInitializer(server);
            jobInitializer.JobInitializationFailed += JobInitializationFailed;
            jobInitializer.JobInitialized += JobInitialized;
            this.serializer = serializer;
        }

        private void JobInitialized(JobInitializer sender, Job job)
        {
            logger.Info("Job initialized: " + job.Name);
            serializer.Save(job.ID, job);
            lock(jobs)
            {
                jobs[job.ID] = job;
            }
        }

        private void JobInitializationFailed(JobInitializer sender, Job job, Exception ex)
        {
            serializer.Save(job.ID, job);
            logger.Warn("Job initialization failed: " + job.Name);
            logger.Warn(ex);
        }

        public void Start()
        {
            //load
            lock(jobs)
            {
                foreach(var j in serializer.LoadAll())
                {
                    if(j.Status == JobStatus.Initializing)
                    {
                        jobInitializer.AddJob(j);
                    }
                    else
                    {
                        jobs[j.ID] = j;
                    }
                }
            }

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

        public void Stop()
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

            //save
            lock(jobs)
            {
                foreach(var j in jobs)
                {
                    serializer.Save(j.Key, j.Value);
                }
            }
        }

        public void AddJob(Job job)
        {
            logger.Info("Adding Job to initializer: " + job.ID);
            serializer.Save(job.ID, job);
            jobInitializer.AddJob(job);
        }

        public void CancelJob(Job job)
        {
            //job.Cancel();
            //TODO:????
        }
    }
}
