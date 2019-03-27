using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Infrastructure.Mvc
{
    public static class MvcExtensions
    {
        public static IServiceCollection ConfigureMvc(this IServiceCollection services, params Type[] filters)
        {
            services
                .AddMvc(options =>
                {
                    foreach (var filter in filters)
                    {
                        options.Filters.Add(filter);
                    }
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return services;
        }

        public static IApplicationBuilder UseMvcMiddleware(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMvc();
        }
    }
}