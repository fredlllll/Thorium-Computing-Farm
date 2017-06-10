using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Codolith.Serialization
{
    class PropertyDataMemberInfo : ADataMemberInfo
    {
        private PropertyInfo prop;

        public PropertyDataMemberInfo(PropertyInfo prop, ReferencingSerializer serializer) : base(serializer)
        {
            this.prop = prop;
        }

        public override string Name
        {
            get
            {
                return prop.Name;
            }
        }

        public override Type MemberType
        {
            get
            {
                return prop.PropertyType;
            }
        }

        public override object GetFromObject(object obj)
        {
            return prop.GetValue(obj);
        }

        public override void SetOnObject(object obj, object value)
        {
            prop.SetValue(obj, value);
        }
    }
}
