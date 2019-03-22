using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Infrastructure.Orm
{
    public static class OrmExtensions
    {
        public static IServiceCollection ConfigureOrm<TStorage>(this IServiceCollection services) 
            where TStorage : DbContext
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<TStorage>(ServiceLifetime.Singleton)
                .BuildServiceProvider();

            return services;
        }
    }
}