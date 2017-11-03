using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Codolith.Config;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared
{
    public class JobInformation : Data.DatabaseObject
    {
        [DataMember]
        public string ID { get; set; }

        public JObject Information { get; private set; } = new JObject();
        [DataMember]
        private string InformationString { get { return Information.ToString(); } set { Information = JObject.Parse(value); } }

        public Type JobType { get; private set; }
        [DataMember]
        private string JobTypeString { get { return JobType.AssemblyQualifiedName; } set { JobType = Type.GetType(value); } }

        public JobInformation(Job job)
        {
            ID = job.ID;
            Information = job.Information;
            JobType = job.GetType();
        }
    }
}
