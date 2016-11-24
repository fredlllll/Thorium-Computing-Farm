using System;
using System.Collections.Generic;
using System.Reflection;

namespace Codolith.Reflection
{
    public class ReflectionHelper
    {
        public static IEnumerable<Type> GetTypeByShortName(string shortName)
        {
            foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = assembly.GetTypes();
                foreach(Type t in types)
                {
                    if(t.Name == shortName)
                    {
                        yield return t;
                    }
                }
            }
        }
    }
}