using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Codolith.Serialization
{
    class FieldDataMemberInfo : ADataMemberInfo
    {
        private FieldInfo field;

        public FieldDataMemberInfo(FieldInfo field, ReferencingSerializer serializer) : base(serializer)
        {
            this.field = field;
        }

        public override string Name
        {
            get
            {
                return field.Name;
            }
        }

        public override Type MemberType
        {
            get
            {
                return field.FieldType;
            }
        }

        public override object GetFromObject(object obj)
        {
            return field.GetValue(obj);
        }

        public override void SetOnObject(object obj, object value)
        {
            field.SetValue(obj, value);
        }
    }
}
