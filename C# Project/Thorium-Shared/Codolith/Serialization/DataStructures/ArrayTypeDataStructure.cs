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

            IsOfPrimitiveType = Utils.IsPrimitiveType(t.GetElementType());
        }

        public ObjectSerializationDataSet GetObjectSerializationDataSet(object obj)
        {
            ObjectSerializationDataSet osds = new ObjectSerializationDataSet();
            osds.TypeID = Serializer.GetTypeID(Type);

            var oaa = (Array)obj;
            int length = oaa.Length;

            Primitive p = new Primitive();
            p.Name = "length";
            p.Value = length;
            osds.AddPrimitive(p);


            if(IsOfPrimitiveType)
            {
                for(int i = 0; i < length; i++)
                {
                    p = new Primitive();
                    p.Name = i.ToString();
                    p.Value = oaa.GetValue(i);
                    osds.AddPrimitive(p);
                }
            }
            else
            {
                for(int i = 0; i < length; i++)
                {
                    p = new Primitive();
                    p.Name = i.ToString();
                    p.Value = Serializer.GetReferenceID(oaa.GetValue(i));
                    osds.AddComplexPrimitive(p);
                }
            }

            return osds;
        }

        public object GetSimpleObject(ObjectSerializationDataSet osds)
        {
            Type t = Serializer.GetType(osds.TypeID);

            int length = (int)osds.GetPrimitive("length").Value;

            var arr = Array.CreateInstance(t.GetElementType(), length);

            if(IsOfPrimitiveType)
            {
                for(int i = 0; i < length; i++)
                {
                    Primitive p = osds.GetPrimitive(i.ToString());
                    arr.SetValue(p.Value, i);
                }
            }

            return arr;
        }

        public void SetComplexMembers(ObjectSerializationDataSet osds, object obj)
        {
            if(!IsOfPrimitiveType)
            {
                var arr = (Array)obj;
                int length = (int)osds.GetPrimitive("length").Value;
                for(int i = 0; i < length; i++)
                {
                    Primitive p = osds.GetComplex(i.ToString());
                    arr.SetValue(Serializer.GetReference((int)p.Value), i);
                }
            }
        }

        public void OnObjectAdd(object obj)
        {
            if(!IsOfPrimitiveType)
            {
                var arr = (Array)obj;
                for(int i = 0; i < arr.Length; i++)
                {
                    Serializer.AddObject(arr.GetValue(i));
                }
            }
        }
    }
}
