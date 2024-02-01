﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium.Shared.DTOs;

namespace Thorium.Server
{
    public class Task
    {
        public Job Job { get; set; }
        public int TaskNumber { get; set; }
        public string Status { get; set; } = "queued"; //TODO: enum

        public Task(Job job, int taskNumber)
        {
            Job = job;
            TaskNumber = taskNumber;
        }

        public ThoriumTask ToThoriumTask()
        {
            return new ThoriumTask() { JobId = Job.ThoriumJob.Id, TaskNumber = TaskNumber };
        }
    }
}
