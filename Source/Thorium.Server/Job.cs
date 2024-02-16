using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium.Shared.DTOs;

namespace Thorium.Server
{
    public class Job
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int TaskCount { get { return tasks.Length; } }
        public OperationDTO[] Operations { get; set; }
        private readonly Task[] tasks;

        private readonly LinkedList<Task> queuedTasks = new();
        private readonly List<Task> runningTasks = new();
        private readonly List<Task> finishedTasks = new();

        public Job(JobDTO job)
        {
            Id = job.Id;
            Name = job.Name;
            Operations = job.Operations;

            tasks = new Task[job.TaskCount];
            for (int i = 0; i < tasks.Length; i++)
            {
                queuedTasks.AddFirst(tasks[i] = new Task(this, i));
            }
        }


        public bool HasTasks
        {
            get { return queuedTasks.Count > 0; }
        }

        public Task GetNextTask()
        {
            if (queuedTasks.Count > 0)
            {
                var task = queuedTasks.First.Value;
                queuedTasks.RemoveFirst();
                runningTasks.Add(task);
                return task;
            }
            return null;
        }

        public void RequeueTask(int taskNum)
        {
            var task = tasks[taskNum];
            if (!runningTasks.Remove(task))
            {
                if (!finishedTasks.Remove(task))
                {
                    return; //task already queued
                }
            }
            queuedTasks.AddLast(task);
        }

        public void TurnInTask(int taskNum, string status)
        {
            var task = tasks[taskNum];
            if (!runningTasks.Remove(task))
            {
                return; //task wasnt running
            }
            task.Status = status;
            finishedTasks.Add(task);
        }

        public JobDTO ToDTO()
        {
            return new JobDTO() { 
                Id = Id,
                Name = Name,
                Operations = Operations,
                TaskCount = TaskCount,
            };
        }
    }
}
