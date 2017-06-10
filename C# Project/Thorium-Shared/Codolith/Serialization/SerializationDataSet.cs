using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codolith.Serialization
{
    public class SerializationDataSet
    {
        public List<TypeDescription> typeDescriptions = new List<TypeDescription>();

        public List<ObjectSerializationDataSet> objectDataSets = new List<ObjectSerializationDataSet>();
    }
}
