using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using static Thorium.Shared.Unused.TaskProperties;

namespace Thorium.Jobs.SimpleExecution
{
    /*public class SETaskProducer : ATaskProducer
    {
        public SETaskProducer(Job job) : base(job)
        {
        }

        public override IEnumerator<Task> GetTasks()
        {
            JObject ji = Job.Information;
            int maxCount = ji.Get<int>("tasksCount");
            for(int i = 0; i < maxCount; i++)
            {
                JObject info = new JObject
                {
                    ["index"] = i,
                    ["executable"] = Job.Information["executable"],
                    ["args"] = Job.Information["args"],
                    [ExecutionerType] = typeof(SEExecutioner).AssemblyQualifiedName
                };

                yield return new Task(Job.ID, Utils.Utils.GetRandomGUID(), info);
            }
        }
    }*/
}
