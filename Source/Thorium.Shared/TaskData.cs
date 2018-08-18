using Newtonsoft.Json.Linq;

namespace Thorium.Shared
{
    public class TaskData
    {
        public string Id { get; protected set; }
        public JObject Information { get; protected set; }
        public TaskStatus Status { get; protected set; }

        public TaskData(string id, JObject information, TaskStatus status = TaskStatus.WaitingForExecution)
        {
            Id = id;
            Information = information;
            Status = status;
        }

        public LightweightTask ToLightweightTask()
        {
            return new LightweightTask(Id, Information);
        }
    }
}
