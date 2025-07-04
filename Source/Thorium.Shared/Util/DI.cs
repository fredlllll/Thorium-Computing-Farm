using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Thorium.Shared.Util
{
    public static class DI
    {
        static HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        public static IServiceCollection Services
        {
            get { return builder.Services; }
        }

        static IServiceProvider serviceProvider = null;
        public static IServiceProvider ServiceProvider
        {
            get
            {
                if (serviceProvider == null)
                {
                    var opt = new ServiceProviderOptions();
                    opt.ValidateOnBuild = true;
                    serviceProvider = builder.Services.BuildServiceProvider(opt);
                }
                return serviceProvider;
            }
        }
    }
}
