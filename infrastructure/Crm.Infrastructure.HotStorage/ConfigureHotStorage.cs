using Crm.Infrastructure.HotStorage.HotStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;

namespace Crm.Infrastructure.HotStorage
{
    public static class MailSendingExtensions
    {
        public static IServiceCollection ConfigureHotStorage(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddSingleton<IHotStorage, HotStorage.HotStorage>()
                .AddSingleton<IRedisClientsManager>(x =>
                    new RedisManagerPool(configuration["HotStorageConnectionString"]));
        }
    }
}