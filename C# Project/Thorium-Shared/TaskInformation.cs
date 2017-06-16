using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Codolith.Config;
using Codolith.Serialization;
using Codolith.Serialization.Formatters;

namespace Thorium_Shared
{
    [DataContract]
    public class TaskInformation
    {
        [DataMember]
        public string JobID { get; set; }
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        private string ConfigString
        {
            get
            {
                using(var ms = new MemoryStream())
                {
                    var rs = new ReferencingSerializer();
                    rs.AddObject(Config);

                    var formatter = new XMLFormatter(ms);
                    formatter.Write(rs.GetSerializationDataSet());

                    return Encoding.UTF8.GetString(ms.GetBuffer());
                }
            }
            set
            {
                using(var ms = new MemoryStream(Encoding.UTF8.GetBytes(value)))
                {
                    var rs = new ReferencingSerializer();

                    var formatter = new XMLFormatter(ms);
                    rs.ReadSerializationDataSet(formatter.Read());

                    foreach(var obj in rs.Objects)
                    {
                        if(obj is Config)
                        {
                            Config = (Config)obj;
                            return;
                        }
                    }
                }
            }
        }
        
        public Config Config { get; private set; } = new Config();

        public Type TaskType
        {
            get
            {
                return Type.GetType(Config.Get(nameof(TaskType)));
            }
            set
            {
                if(!value.IsSubclassOf(typeof(ATask)))
                {
                    throw new ArgumentException("the type has to be a subclass of " + nameof(ATask));
                }
                Config.Set(nameof(TaskType), value.AssemblyQualifiedName);
            }
        }
    }
}
