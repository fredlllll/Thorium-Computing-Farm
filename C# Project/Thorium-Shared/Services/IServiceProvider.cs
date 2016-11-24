using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Services
{
    public interface IServiceProvider<T> where T : IService
    {
        U GetService<U>() where U : T;
        T GetService(Type type);
    }
}
