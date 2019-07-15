using Crm.Infrastructure.Orm.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Infrastructure.Orm
{
    public static class OrmExtensions
    {
        public static IServiceCollection ConfigureOrm<TStorage>(
            this IServiceCollection services,
            IConfiguration configuration) where TStorage : Storage
        {
            var section = configuration.GetSection("OrmSettings");
            
            services.Configure<OrmSettings>(section);

            var isTestMode = section.GetValue<bool>("IsTestMode");
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