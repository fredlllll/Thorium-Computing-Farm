using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codolith.Serialization
{
    public class ObjectSerializationDataSet
    {
        public int TypeID { get; set; } = -1;

        public IEnumerable<Primitive> Primitives { get { return primitives; } }
        public int PrimitiveCount { get { return primitives.Count; } }
        private List<Primitive> primitives = new List<Primitive>();
        public IEnumerable<Primitive> ComplexPrimitives { get { return complexPrimitives; } }
        public int ComplexCount { get { return complexPrimitives.Count; } }
        private List<Primitive> complexPrimitives = new List<Primitive>();

        private Dictionary<string, Primitive> primitiveNames = new Dictionary<string, Primitive>();
        private Dictionary<string, Primitive> complexNames = new Dictionary<string, Primitive>();

        public void AddPrimitive(Primitive p)
        {
            primitives.Add(p);
            primitiveNames[p.Name] = p;
        }

        public void AddComplexPrimitive(Primitive p)
        {
            complexPrimitives.Add(p);
            complexNames[p.Name] = p;
        }

        public Primitive GetPrimitive(string name)
        {
            return primitiveNames[name];
        }

        public Primitive GetComplex(string name)
        {
            return complexNames[name];
        }
    }
}
