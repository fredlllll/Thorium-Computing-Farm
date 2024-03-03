using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Thorium.Server.Models;
using Thorium.Shared.DTOs;

namespace Thorium.Server
{
    public static class JobUtil
    {
        public static Job CreateFromDTO(JobDTO jobDto)
        {
            var job = new Job()
            {
                Id = jobDto.Id,
                Name = jobDto.Name,
                Operations = jobDto.Operations,
            };

            for (int i = 0; i < job.TaskCount; i++)
            {
                TaskUtil.CreateTask(job.Id, i);
            }
            return job;
        }

        public static Task GetNextTask(Job job)
        {
            /*if (queuedTasks.Count > 0)
            {
                var task = queuedTasks.First.Value;
                queuedTasks.RemoveFirst();
                runningTasks.Add(task);
                return task;
            }*/
            return null;
        }

        public static void RequeueTask(Job job, int taskNum)
        {
            /*var task = tasks[taskNum];
            if (!runningTasks.Remove(task))
            {
                if (!finishedTasks.Remove(task))
                {
                    return; //task already queued
                }
            }
            queuedTasks.AddLast(task);*/
        }

        public static void TurnInTask(Job job, int taskNum, string status)
        {
            /*var task = tasks[taskNum];
            if (!runningTasks.Remove(task))
            {
                return; //task wasnt running
            }
            task.Status = status;
            finishedTasks.Add(task);*/
        }

        public static JobDTO ToDto(Job job)
        {
            return new JobDTO()
            {
                Id = job.Id,
                Name = job.Name,
                Operations = job.Operations,
                TaskCount = job.TaskCount,
            };
        }
    }
}
