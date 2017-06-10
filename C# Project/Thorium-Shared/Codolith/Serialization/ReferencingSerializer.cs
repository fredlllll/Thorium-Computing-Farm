using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codolith.Serialization.DataStructures;

namespace Codolith.Serialization
{
    public class ReferencingSerializer
    {
        internal MultiDictionary<int, Type, ITypeDataStructure> types = new MultiDictionary<int, Type, ITypeDataStructure>();

        internal MultiDictionary<object, int> references = new MultiDictionary<object, int>();

        private List<object> objects = new List<object>();
        public IEnumerable<object> Objects
        {
            get
            {
                return objects;
            }
        }

        public void AddObject(object obj)
        {
            if(obj == null)
            {
                return;
            }
            if(references.ContainsKey(obj))
            {
                return;
            }

            ITypeDataStructure tds = GetTypeDataStructure(obj.GetType());

            ModTuple<object, int> reference = new ModTuple<object, int>(obj, references.Count);
            references[obj] = reference;
            objects.Add(obj);

            if(tds.ComplexMembers != null)
            {
                foreach(var member in tds.ComplexMembers)
                {
                    object value = member.GetFromObject(obj);
                    if(value != null)
                    {
                        AddObject(value);
                    }
                }
            }
        }

        public void SetTypeDataStructure(Type t, ITypeDataStructure tds)
        {
            if(types.ContainsKey(t))
            {
                types[t].Value3 = tds;
            }
            else
            {
                types[types.Count] = new ModTuple<int, Type, ITypeDataStructure>(types.Count, t, tds);
            }
        }

        public SerializationDataSet GetSerializationDataSet()
        {
            SerializationDataSet sds = new SerializationDataSet();
            foreach(var kv in types)
            {
                sds.typeDescriptions.Add(new TypeDescription(kv.Value.Value2.AssemblyQualifiedName));
            }

            foreach(var kv in references)
            {
                ITypeDataStructure tds = GetTypeDataStructure(kv.Key.GetType());
                sds.objectDataSets.Add(tds.GetObjectSerializationDataSet(kv.Key));
            }

            return sds;
        }

        public void ReadSerializationDataSet(SerializationDataSet sds)
        {
            foreach(var td in sds.typeDescriptions)
            {
                Type t = Type.GetType(td.TypeName);
                GetTypeDataStructure(t);
            }

            Dictionary<object, ObjectSerializationDataSet> tdss = new Dictionary<object, ObjectSerializationDataSet>();
            foreach(var ods in sds.objectDataSets)
            {
                Type t = GetType(ods.TypeIndex);
                var tds = GetTypeDataStructure(t);
                object obj = tds.GetSimpleObject(ods);

                ModTuple<object, int> reference = new ModTuple<object, int>(obj, references.Count);
                references[obj] = reference;
                objects.Add(obj);
                tdss[obj] = ods;
            }

            foreach(var kv in tdss)
            {
                var tds = GetTypeDataStructure(kv.Key.GetType());
                tds.SetComplexMembers(kv.Value, kv.Key);
            }
        }

        internal int GetReferenceID(object obj)
        {
            if(obj == null)
            {
                return 0;
            }

            ModTuple<object, int> reference;
            if(references.TryGetValue(obj, out reference))
            {
                return reference.Value2;
            }

            AddObject(obj);
            return GetReferenceID(obj);
        }

        internal object GetReference(int id)
        {
            return references[id].Value1;
        }

        internal int GetTypeID(Type t)
        {
            return types[t].Value1;
        }

        internal Type GetType(int id)
        {
            return types[id].Value2;
        }

        internal ITypeDataStructure GetTypeDataStructure(Type t)
        {
            ModTuple<int, Type, ITypeDataStructure> mt;
            if(types.TryGetValue(t, out mt))
            {
                return mt.Value3;
            }

            ITypeDataStructure tds;
            if(t.IsEnum)
            {
                tds = new EnumTypeDataStructure(t, this);
            }
            else
            {
                tds = new DefaultTypeDataStructure(t, this);
            }
            types[types.Count] = new ModTuple<int, Type, ITypeDataStructure>(types.Count, t, tds);

            return tds;
        }
    }
}
