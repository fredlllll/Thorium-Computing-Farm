using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Codolith.Serialization.DataStructures
{
    internal class DefaultTypeDataStructure : ITypeDataStructure
    {
        public Type Type
        {
            get; private set;
        }
        public ReferencingSerializer Serializer
        {
            get; private set;
        }

        List<ADataMemberInfo> complexDataMembers = new List<ADataMemberInfo>();
        List<ADataMemberInfo> primitiveDataMembers = new List<ADataMemberInfo>();

        public IEnumerable<ADataMemberInfo> ComplexMembers { get { return complexDataMembers; } }

        public IEnumerable<ADataMemberInfo> PrimitiveMembers { get { return primitiveDataMembers; } }

        public DefaultTypeDataStructure(Type type, ReferencingSerializer serializer)
        {
            this.Type = type;
            this.Serializer = serializer;
            AnalyzeType();
        }

        private void AnalyzeType()
        {
            FieldInfo[] fields = Type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach(var field in fields)
            {
                if(field.IsDefined(typeof(DataMemberAttribute)))
                {
                    AddMember(new FieldDataMemberInfo(field, Serializer));
                }
            }

            PropertyInfo[] properties = Type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach(var prop in properties)
            {
                if(prop.IsDefined(typeof(DataMemberAttribute)))
                {
                    AddMember(new PropertyDataMemberInfo(prop, Serializer));
                }
            }
        }

        private void AddMember(ADataMemberInfo dmi)
        {
            if(dmi.IsPrimitive)
            {
                primitiveDataMembers.Add(dmi);
            }
            else
            {
                complexDataMembers.Add(dmi);
            }
        }

        public IEnumerable<Primitive> GetComplexPrimitives(object obj)
        {
            List<Primitive> prims = new List<Primitive>();
            foreach(var member in complexDataMembers)
            {
                prims.Add(member.GetPrimitive(obj));
            }
            return prims;
        }

        public IEnumerable<Primitive> GetPrimitives(object obj)
        {
            List<Primitive> prims = new List<Primitive>();
            foreach(var member in primitiveDataMembers)
            {
                prims.Add(member.GetPrimitive(obj));
            }
            return prims;
        }

        public ObjectSerializationDataSet GetObjectSerializationDataSet(object obj)
        {
            ObjectSerializationDataSet osds = new ObjectSerializationDataSet();
            osds.TypeIndex = Serializer.GetTypeID(Type);
            foreach(var p in GetComplexPrimitives(obj))
            {
                osds.complexPrimitives.Add(p);
            }
            foreach(var p in GetPrimitives(obj))
            {
                osds.primitives.Add(p);
            }
            return osds;
        }

        private ADataMemberInfo GetPrimitiveByName(string name) {
            foreach(var dm in primitiveDataMembers)
            {
                if(dm.Name == name)
                {
                    return dm;
                }
            }
            return default(ADataMemberInfo);
        }

        private ADataMemberInfo GetComplexByName(string name) {
            foreach(var dm in complexDataMembers)
            {
                if(dm.Name == name)
                {
                    return dm;
                }
            }
            return default(ADataMemberInfo);
        }

        public object GetSimpleObject(ObjectSerializationDataSet osds)
        {
            Type t = Serializer.GetType(osds.TypeIndex);
            object retval = Utils.GetUninitializedInstance(t);

            foreach(var prim in osds.primitives)
            {
                var dm = GetPrimitiveByName(prim.Name);
                dm.SetOnObject(retval, prim.Value);
            }

            return retval;
        }

        public void SetComplexMembers(ObjectSerializationDataSet osds,object obj)
        {
            foreach(var prim in osds.complexPrimitives)
            {
                object other = Serializer.GetReference((int)prim.Value);
                var dm = GetComplexByName(prim.Name);
                dm.SetOnObject(obj, other);
            }
        }
    }
}