using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    /// <summary>
    /// initializes static stuff i guess. still has to be called though
    /// </summary>
    public static class ModuleInitializer
    {
        public static void Initialize()
        {
            //add bindings
            DependencyInjection.Kernel.Bind(null).To(null).InSingletonScope();
        }
    }
}
