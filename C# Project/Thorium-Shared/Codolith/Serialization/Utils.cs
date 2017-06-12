using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Codolith.Serialization
{
    public static class Utils
    {
        private static List<Type> primitiveTypes = new List<Type>();
        public static IEnumerable<Type> PrimitiveTypes
        {
            get
            {
                return primitiveTypes;
            }
        }

        static Utils()
        {
            primitiveTypes.Add(typeof(sbyte));
            primitiveTypes.Add(typeof(byte));
            primitiveTypes.Add(typeof(short));
            primitiveTypes.Add(typeof(ushort));
            primitiveTypes.Add(typeof(int));
            primitiveTypes.Add(typeof(uint));
            primitiveTypes.Add(typeof(long));
            primitiveTypes.Add(typeof(ulong));

            primitiveTypes.Add(typeof(float));
            primitiveTypes.Add(typeof(double));

            primitiveTypes.Add(typeof(decimal));

            primitiveTypes.Add(typeof(string));
            primitiveTypes.Add(typeof(char));

            primitiveTypes.Add(typeof(bool));
        }

        public static bool IsPrimitiveType(Type t)
        {
            return t.IsPrimitive || t == typeof(string) || t == typeof(decimal);
        }

        public static object GetDefaulInstanceOrUninitialized(Type t)
        {
            var constr = t.GetConstructor(Type.EmptyTypes);
            if(constr != null)
            {
                return constr.Invoke(new object[0]);
            }
            return FormatterServices.GetSafeUninitializedObject(t);
        }

        public static object GetUninitializedInstance(Type t)
        {
            return FormatterServices.GetSafeUninitializedObject(t);
        }

        public static T GetUninitializedInstance<T>(Type t)
        {
            return (T)FormatterServices.GetSafeUninitializedObject(t);
        }

        public static T GetUninitializedInstance<T>()
        {
            return GetUninitializedInstance<T>(typeof(T));
        }
    }
}
