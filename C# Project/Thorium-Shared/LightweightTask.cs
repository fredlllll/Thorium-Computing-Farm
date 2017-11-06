using System;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared
{
    public class LightweightTask
    {
        public string JobID { get; set; }
        public string ID { get; set; }
        public JObject Information { get; set; }

        public LightweightTask(Task t)
        {
            JobID = t.Job.ID;
            ID = t.ID;
            Information = t.Information;
        }

        public LightweightTask() { }

        public AExecutioner GetExecutioner()
        {
            string typeName = Information.Get<string>("executioner");
            Type t = Type.GetType(typeName);
            var ci = t.GetConstructor(new Type[] { typeof(LightweightTask) });
            return (AExecutioner)ci.Invoke(new object[] { this });
        }
    }
}
