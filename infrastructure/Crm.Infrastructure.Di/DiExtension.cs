using Microsoft.Extensions.DependencyInjection;

namespace Crm.Infrastructure.Di
{
    public static class DiExtension
    {
        public static IServiceCollection GetServiceCollection()
        {
            return new ServiceCollection();
        }
        
       public static ServiceProvider GetServiceProvider(this IServiceCollection services)
        {
            return services
                .BuildServiceProvider();
        }
    }
}