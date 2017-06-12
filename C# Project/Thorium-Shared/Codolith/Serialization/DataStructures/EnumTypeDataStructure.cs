using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codolith.Serialization;

namespace Codolith.Serialization.DataStructures
{
    internal class EnumTypeDataStructure : ITypeDataStructure
    {
        public ReferencingSerializer Serializer { get; private set; }
        public IEnumerable<ADataMemberInfo> ComplexMembers { get { return null; } }
        public Type Type { get; private set; }

        List<ADataMemberInfo> primitiveMembers = new List<ADataMemberInfo>();
        public IEnumerable<ADataMemberInfo> PrimitiveMembers { get { return primitiveMembers; } }

        public EnumTypeDataStructure(Type t, ReferencingSerializer serializer)
        {
            Type = t;
            Serializer = serializer;
        }

        public ObjectSerializationDataSet GetObjectSerializationDataSet(object obj)
        {
            ObjectSerializationDataSet osds = new ObjectSerializationDataSet();
            osds.TypeID = Serializer.GetTypeID(Type);

            Primitive p = new Primitive();
            p.Name = "value";
            p.Value = Enum.GetName(Type, obj);
            osds.AddPrimitive(p);

            return osds;
        }

        public object GetSimpleObject(ObjectSerializationDataSet osds)
        {
            return Enum.Parse(Type, (string)osds.GetPrimitive("value").Value);
        }

        public void SetComplexMembers(ObjectSerializationDataSet osds, object obj)
        {

        }

        public void OnObjectAdd(object obj)
        {
           
        }
    }
}
