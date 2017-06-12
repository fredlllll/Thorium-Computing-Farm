using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codolith.Serialization.DataStructures
{
    internal class IListTypeDataStructure : ITypeDataStructure
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

        private bool IsOfPrimitiveType { get; set; }
        public IListTypeDataStructure(Type t, ReferencingSerializer serializer)
        {
            Serializer = serializer;
            Type = t;

            IsOfPrimitiveType = Utils.IsPrimitiveType(t.GenericTypeArguments[0]);
        }

        public ObjectSerializationDataSet GetObjectSerializationDataSet(object obj)
        {
            ObjectSerializationDataSet osds = new ObjectSerializationDataSet();
            osds.TypeID = Serializer.GetTypeID(obj.GetType());

            IList list = (IList)obj;

            Primitive p = new Primitive();
            p.Name = "count";
            p.Value = list.Count;
            osds.AddPrimitive(p);

            Action<IList, int, ObjectSerializationDataSet> act = AddComplex;
            if(IsOfPrimitiveType)
            {
                act = AddPrimitive;
            }

            for(int i = 0; i < list.Count; i++)
            {
                act(list, i, osds);
            }

            return osds;
        }

        private void AddPrimitive(IList list, int index, ObjectSerializationDataSet osds)
        {
            Primitive p = new Primitive();
            p.Name = index.ToString();
            p.Value = list[index];

            osds.AddPrimitive(p);
        }

        private void AddComplex(IList list, int index, ObjectSerializationDataSet osds)
        {
            Primitive p = new Primitive();
            p.Name = index.ToString();
            p.Value = Serializer.GetReferenceID(list[index]);

            osds.AddComplexPrimitive(p);
        }

        public object GetSimpleObject(ObjectSerializationDataSet osds)
        {
            Type t = Serializer.GetType(osds.TypeID);
            IList list = (IList)Utils.GetDefaulInstanceOrUninitialized(t);

            if(IsOfPrimitiveType)
            {
                int count = (int)osds.GetPrimitive("count").Value;
                for(int i = 0; i < count; i++)
                {
                    Primitive p = osds.GetPrimitive(i.ToString());
                    list.Add(p.Value);
                }
            }
            return list;
        }

        public void SetComplexMembers(ObjectSerializationDataSet osds, object obj)
        {
            if(!IsOfPrimitiveType)
            {
                IList list = (IList)obj;
                int count = (int)osds.GetPrimitive("count").Value;
                for(int i = 0; i < count; i++)
                {
                    Primitive p = osds.GetComplex(i.ToString());
                    list.Add(Serializer.GetReference((int)p.Value));
                }
            }
        }

        public void OnObjectAdd(object obj)
        {
            if(!IsOfPrimitiveType)
            {
                var list = (IList)obj;
                for(int i = 0; i < list.Count; i++)
                {
                    Serializer.AddObject(list[i]);
                }
            }
        }
    }
}
