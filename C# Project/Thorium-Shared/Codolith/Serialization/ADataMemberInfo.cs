using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codolith.Serialization
{
    public abstract class ADataMemberInfo
    {
        protected ReferencingSerializer serializer;

        public abstract string Name { get; }
        public abstract Type MemberType { get; }
        public bool IsPrimitive { get { return Utils.IsPrimitiveType(MemberType); } }

        public ADataMemberInfo(ReferencingSerializer serializer)
        {
            this.serializer = serializer;
        }

        public abstract object GetFromObject(object obj);
        public abstract void SetOnObject(object obj, object value);

        public Type GetValueType(object obj)
        {
            object val = GetFromObject(obj);
            if(val != null)
            {
                return val.GetType();
            }
            return null;
        }

        /// <summary>
        /// gets value type if value is not null, else gets member type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Type GetAccurateType(object obj)
        {
            Type t = GetValueType(obj);
            if(t != null)
            {
                return t;
            }
            return MemberType;
        }

        public Primitive GetPrimitive(object obj)
        {
            if(IsPrimitive)
            {
                return new Primitive() { Name = Name, Value = GetFromObject(obj) };
            }
            else
            {
                return new Primitive() { Name = Name, Value = serializer.GetReferenceID(GetFromObject(obj)) };
            }
        }

        public void ApplyPrimitive(Primitive prim, object obj)
        {
            if(IsPrimitive)
            {
                SetOnObject(obj, prim.Value);
            }
            else
            {
                object value = serializer.GetReference((int)prim.Value);
                SetOnObject(obj, value);
            }
        }
    }
}
