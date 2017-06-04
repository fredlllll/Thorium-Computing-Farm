using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Codolith.Config;

namespace Thorium_Shared
{
    [DataContract]
    public class JobInformation
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public Config Config { get; } = new Config();

        public Type JobType
        {
            get
            {
                return Type.GetType(Config.Get(nameof(JobType)));
            }
            set
            {
                if(!value.IsSubclassOf(typeof(AJob)))
                {
                    throw new ArgumentException("the type has to be a subclass of " + nameof(AJob));
                }
                Config.Set(nameof(JobType), value.AssemblyQualifiedName);
            }
        }
    }
}
