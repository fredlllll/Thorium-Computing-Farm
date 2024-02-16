using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Thorium.Shared.Aether
{
    public class DefaultSerializer
    {
        public object ReadFrom(AetherStream aetherStream, Type type)
        {
            var value = RuntimeHelpers.GetUninitializedObject(type);

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    property.SetValue(value, aetherStream.Read());
                }
            }

            return value;
        }

        public void WriteTo(AetherStream aetherStream, object value, Type type)
        {
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    aetherStream.Write(property.GetValue(value));
                }
            }
        }
    }
}
