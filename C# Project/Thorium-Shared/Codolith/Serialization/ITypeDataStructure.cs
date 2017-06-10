using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codolith.Serialization
{
    public interface ITypeDataStructure
    {
        Type Type { get; }
        ReferencingSerializer Serializer { get; }

        IEnumerable<ADataMemberInfo> ComplexMembers { get; }
        IEnumerable<ADataMemberInfo> PrimitiveMembers { get; }

        ObjectSerializationDataSet GetObjectSerializationDataSet(object obj);

        object GetSimpleObject(ObjectSerializationDataSet osds);
        void SetComplexMembers(ObjectSerializationDataSet osds, object obj);
    }
}
