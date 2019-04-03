using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit.DependencyInjection;
using Xunit.Sdk;

namespace Crm.Infrastructure.Di
{
    public class BaseStartup : DependencyInjectionTestFramework
    {
        public BaseStartup()
            : base(new NullMessageSink())
        {
        }

        protected sealed override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Configure(services);

            return services.BuildServiceProvider();
        }

        protected virtual void Configure(IServiceCollection services)
        {
        }
    }
}