using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Services
{
    public abstract class AServiceManager<T> : IServiceProvider<T> where T : IService 
    {
        Dictionary<Type, T> services = new Dictionary<Type, T>();

        public T GetService(Type type)
        {
            T retval;
            if(services.TryGetValue(type, out retval))
            {
                return retval;
            }
            try
            {
                retval = GetNewInstance(type);
                return retval;
            }
            catch
            {
                //not in my hands i guess TODO:log?
            }
            return default(T);
        }

        public U GetService<U>() where U : T
        {
            return (U)GetService(typeof(U));
        }

        protected abstract T GetNewInstance(Type type);
    }
}
