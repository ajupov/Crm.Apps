using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Infrastructure.Orm
{
    public static class OrmExtensions
    {
        public static IServiceCollection ConfigureOrm<TStorage>(this IServiceCollection services,
            IConfiguration configuration) where TStorage : DbContext
        {
            services.Configure<OrmSettings>(configuration.GetSection("OrmSettings"))
                .AddEntityFrameworkNpgsql().AddDbContext<TStorage>(ServiceLifetime.Singleton)
                .BuildServiceProvider();

            return services;
        }
    }
}