using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codolith.Serialization
{
    public class ObjectSerializationDataSet
    {
        public int TypeIndex { get; set; }
        public List<Primitive> primitives = new List<Primitive>();
        public List<Primitive> complexPrimitives = new List<Primitive>();
    }
}
