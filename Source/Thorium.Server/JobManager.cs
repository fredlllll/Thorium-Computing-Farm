using System;
using Thorium.Shared;
using NLog;
using Thorium.Data.Implementation.Serializers;

namespace Thorium.Server
{
    public class JobManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly JobInitializer jobInitializer;

        private readonly JobSerializer jobSerializer;
        private readonly TaskSerializer taskSerializer;

        public JobManager(ThoriumServer server, JobSerializer serializer, TaskSerializer taskSerializer)
        {
            jobInitializer = new JobInitializer(server);
            jobInitializer.JobInitializationFailed += JobInitializationFailed;
            jobInitializer.JobInitialized += JobInitialized;

            this.jobSerializer = serializer;
            this.taskSerializer = taskSerializer;
        }

        private void JobInitialized(JobInitializer sender, Job job)
        {
            logger.Info("Job initialized: " + job.Name);
            jobSerializer.Save(job.ID, job);
        }

        private void JobInitializationFailed(JobInitializer sender, Job job, Exception ex)
        {
            jobSerializer.Save(job.ID, job);
            logger.Warn("Job initialization failed: " + job.Name);
            logger.Warn(ex);
        }

        public void Start()
        {
            //load
            foreach(var j in jobSerializer.LoadWhere("status", JobStatus.Initializing.ToString()))
            {
                taskSerializer.DeleteWhere("job_id", j.ID);//delete tasks that were saved previously, but job init wasnt completed
                jobInitializer.AddJob(j);
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
        }

        public void AddJob(Job job)
        {
            logger.Info("Adding Job to initializer: " + job.ID);
            jobSerializer.Save(job.ID, job);
            jobInitializer.AddJob(job);
        }

        public void CancelJob(Job job)
        {
            //job.Cancel();
            //TODO:????
        }
    }
}
