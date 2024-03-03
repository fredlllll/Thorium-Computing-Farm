using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thorium.Server.Models;
using Thorium.Shared.DTOs;

namespace Thorium.Server
{
    public class TaskUtil
    {
        public static Task CreateTask(string jobId, int taskNumber)
        {
            var task = new Task()
            {
                JobId = jobId,
                Status = "queued",
                TaskNumber = taskNumber,
            };
            return task;
        }

        public static TaskDTO ToDto(Task task)
        {
            return new TaskDTO() { JobId = task.JobId, TaskNumber = task.TaskNumber };
        }
    }
}
