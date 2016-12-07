using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Thorium_Shared.Services;

namespace Thorium_Shared
{
    public static class DependencyInjection
    {
        public static IKernel Kernel
        {
            get;
            private set;
        }

        static DependencyInjection()
        {
            Kernel = new StandardKernel();
        }
    }
}
