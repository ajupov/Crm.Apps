﻿using Crm.Infrastructure.Orm.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Infrastructure.Orm
{
    public static class OrmExtensions
    {
        public static IServiceCollection ConfigureOrm<TStorage>(this IServiceCollection services,
            IConfiguration configuration) where TStorage : Storage
        {
            services.Configure<OrmSettings>(configuration.GetSection("OrmSettings"));

            var isTestMode = configuration.GetSection("OrmSettings").GetValue<bool>("IsTestMode");
            if (isTestMode)
            {
                services.AddEntityFrameworkInMemoryDatabase();
            }
            else
            {
                services.AddEntityFrameworkNpgsql();
            }

            return services.AddDbContext<TStorage>(ServiceLifetime.Transient);
        }
    }
}