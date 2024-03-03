using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium.Shared.DTOs
{
    public enum TaskStatus
    {
        Queued,
        Running,
        Finished
    }

    public class TaskDTO
    {
        public string Id { get; set; }
        public string JobId { get; set; }
        public int TaskNumber { get; set; }
        public TaskStatus Status { get; set; }
    }
}
