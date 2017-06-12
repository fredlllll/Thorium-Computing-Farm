using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codolith.Serialization;

namespace Codolith.Serialization.DataStructures
{
    internal class IDictionaryTypeDataStructure : ITypeDataStructure
    {
        List<ADataMemberInfo> complexMembers = new List<ADataMemberInfo>();
        public IEnumerable<ADataMemberInfo> ComplexMembers
        {
            get
            {
                return complexMembers;
            }
        }

        List<ADataMemberInfo> primitiveMembers = new List<ADataMemberInfo>();
        public IEnumerable<ADataMemberInfo> PrimitiveMembers
        {
            get
            {
                return primitiveMembers;
            }
        }

        public ReferencingSerializer Serializer { get; private set; }
        public Type Type { get; private set; }

        private bool IsKeyOfPrimitiveType { get; set; }
        private bool IsValueOfPrimitiveType { get; set; }
        public IDictionaryTypeDataStructure(Type t, ReferencingSerializer serializer)
        {
            Serializer = serializer;
            Type = t;

            IsKeyOfPrimitiveType = Utils.IsPrimitiveType(t.GenericTypeArguments[0]);
            IsValueOfPrimitiveType = Utils.IsPrimitiveType(t.GenericTypeArguments[1]);
        }

        public ObjectSerializationDataSet GetObjectSerializationDataSet(object obj)
        {
            ObjectSerializationDataSet osds = new ObjectSerializationDataSet();
            osds.TypeID = Serializer.GetTypeID(obj.GetType());

            IDictionary dict = (IDictionary)obj;

            Primitive p = new Primitive();
            p.Name = "count";
            p.Value = dict.Count;
            osds.AddPrimitive(p);

            int i = 0;
            var enumerator = dict.GetEnumerator();
            while(enumerator.MoveNext())
            {
                AddKey(enumerator.Key, i, osds);
                AddValue(enumerator.Value, i, osds);
                i++;
            }

            return osds;
        }

        private void AddKey(object key, int index, ObjectSerializationDataSet osds)
        {
            var p = new Primitive();
            p.Name = "key" + index;
            if(IsKeyOfPrimitiveType)
            {
                p.Value = key;
                osds.AddPrimitive(p);
            }
            else
            {
                p.Value = Serializer.GetReferenceID(key);
                osds.AddComplexPrimitive(p);
            }
        }

        private void AddValue(object value, int index, ObjectSerializationDataSet osds)
        {
            var p = new Primitive();
            p.Name = "value" + index;
            if(IsValueOfPrimitiveType)
            {
                p.Value = value;
                osds.AddPrimitive(p);
            }
            else
            {
                p.Value = Serializer.GetReferenceID(value);
                osds.AddComplexPrimitive(p);
            }
        }

        public object GetSimpleObject(ObjectSerializationDataSet osds)
        {
            Type t = Serializer.GetType(osds.TypeID);
            IDictionary list = (IDictionary)Utils.GetDefaulInstanceOrUninitialized(t);

            if(IsKeyOfPrimitiveType && IsValueOfPrimitiveType)
            {
                int count = (int)osds.GetPrimitive("count").Value;
                for(int i = 0; i < count; i++)
                {
                    object key = osds.GetPrimitive("key" + i).Value;
                    object value = osds.GetPrimitive("value" + i).Value;
                    list[key] = value;
                }
            }
            return list;
        }

        public void SetComplexMembers(ObjectSerializationDataSet osds, object obj)
        {
            if(!IsKeyOfPrimitiveType || !IsValueOfPrimitiveType)
            {
                IDictionary list = (IDictionary)obj;
                int count = (int)osds.GetPrimitive("count").Value;
                for(int i = 0; i < count; i++)
                {
                    object key;
                    if(IsKeyOfPrimitiveType)
                    {
                        key = osds.GetPrimitive("key" + i).Value;
                    }
                    else
                    {
                        key = Serializer.GetReference((int)osds.GetComplex("key" + i).Value);
                    }
                    object value;
                    if(IsValueOfPrimitiveType)
                    {
                        value = osds.GetPrimitive("value" + i).Value;
                    }
                    else
                    {
                        value = Serializer.GetReference((int)osds.GetComplex("value" + i).Value);
                    }
                    list[key] = value;
                }
            }
        }

        public void OnObjectAdd(object obj)
        {
            if(!IsKeyOfPrimitiveType || !IsValueOfPrimitiveType)
            {
                var dict = (IDictionary)obj;
                var enumerator = dict.GetEnumerator();
                while(enumerator.MoveNext())
                {
                    if(!IsKeyOfPrimitiveType)
                    {
                        Serializer.AddObject(enumerator.Key);
                    }
                    if(!IsValueOfPrimitiveType)
                    {
                        Serializer.AddObject(enumerator.Value);
                    }
                }
            }
        }
    }
}
