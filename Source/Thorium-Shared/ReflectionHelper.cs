using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Thorium_Shared
{
    public class ReflectionHelper
    {
        /// <summary>
        /// searches using name, namespace+name and assembly qualified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Type GetType(string name)
        {
            foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = assembly.GetTypes();
                foreach(Type t in types)
                {
                    if(t.Name == name || t.FullName == name || t.AssemblyQualifiedName == name)
                    {
                        return t;
                    }
                }
            }

            return Type.GetType(name);
        }

        public static object GetDefault(Type type)
        {
            if(type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public static bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// returns the type that contains the member that called your method
        /// </summary>
        /// <returns></returns>
        public static Type GetCallingType()
        {
            MethodBase method;
            for(int num = 2; ; num++)
            {
                StackFrame stackFrame = new StackFrame(num, false);
                method = stackFrame.GetMethod();
                Type declaringType = method.DeclaringType;
                if(declaringType == null)
                {
                    break;
                }
                if(!declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase))
                {
                    return declaringType;
                }
            }
            return method.DeclaringType;
        }

        /// <summary>
        /// gets the method base that is calling your method
        /// </summary>
        /// <returns></returns>
        private static MethodBase GetCallingMethod()
        {
            StackTrace trace = new StackTrace();
            for(int offset = 2; ; offset++)
            {
                var frame = trace.GetFrame(offset);
                var method = frame.GetMethod();
                if(method.DeclaringType == null)
                {
                    return null;
                }
                if(!method.DeclaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase))
                {
                    return method;
                }
            }
        }
    }
}