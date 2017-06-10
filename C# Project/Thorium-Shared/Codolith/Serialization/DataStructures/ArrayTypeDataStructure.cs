using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codolith.Serialization;

namespace Codolith.Serialization.DataStructures
{
    internal class ArrayTypeDataStructure : ITypeDataStructure
    {
        public ReferencingSerializer Serializer { get; private set; }

        public Type Type { get; private set; }

        List<ADataMemberInfo> complexMembers = new List<ADataMemberInfo>();
        public IEnumerable<ADataMemberInfo> ComplexMembers { get { return null; } }
        List<ADataMemberInfo> primitiveMembers = new List<ADataMemberInfo>();
        public IEnumerable<ADataMemberInfo> PrimitiveMembers { get { return primitiveMembers; } }

        public bool IsOfPrimitiveType { get; private set; }

        public ArrayTypeDataStructure(Type t, ReferencingSerializer serializer)
        {
            Type = t;
            Serializer = serializer;

            IsOfPrimitiveType = Utils.IsTypePrimitive(t.GetElementType());
        }

        public ObjectSerializationDataSet GetObjectSerializationDataSet(object obj)
        {
            ObjectSerializationDataSet osds = new ObjectSerializationDataSet();
            osds.TypeIndex = Serializer.GetTypeID(Type);

            var oaa = (Array)obj;
            int length = oaa.Length;

            Primitive p = new Primitive();
            p.Name = "length";
            p.Value = length;
            osds.primitives.Add(p);


            if(IsOfPrimitiveType)
            {
                for(int i = 0; i < length; i++)
                {
                    p = new Primitive();
                    p.Name = i.ToString();
                    p.Value = oaa.GetValue(i);
                    osds.primitives.Add(p);
                }
            }
            else
            {
                for(int i = 0; i < length; i++)
                {
                    p = new Primitive();
                    p.Name = i.ToString();
                    p.Value = Serializer.GetReferenceID(oaa.GetValue(i));
                    osds.complexPrimitives.Add(p);
                }
            }

            return osds;
        }

        public object GetSimpleObject(ObjectSerializationDataSet osds)
        {
            Type t = Serializer.GetType(osds.TypeIndex);

            int length = (int)osds.primitives.Find((x) => { return x.Name == "length"; }).Value;

            var arr = Array.CreateInstance(t.GetElementType(), length);

            if(IsOfPrimitiveType)
            {
                foreach(var p in osds.primitives)
                {
                    int index;
                    if(int.TryParse(p.Name, out index))
                    {
                        arr.SetValue(p.Value, index);
                    }
                }
            }

            return arr;
        }

        public void SetComplexMembers(ObjectSerializationDataSet osds, object obj)
        {
            var arr = (Array)obj;

            if(!IsOfPrimitiveType)
            {
                foreach(var p in osds.complexPrimitives)
                {
                    int index;
                    if(int.TryParse(p.Name, out index))
                    {
                        arr.SetValue(Serializer.GetReference((int)p.Value), index);
                    }
                }
            }
        }
    }
}
