using Crm.Infrastructure.MessageBroking.Consuming;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crm.Infrastructure.MessageBroking
{
    public static class MessageBrokingExtensions
    {
        public static IServiceCollection ConfigureConsumer<TConsumer>(this IServiceCollection services,
            IConfiguration configuration) where TConsumer : class, IHostedService
        {
            return services
                .Configure<ConsumerSettings>(configuration.GetSection("MessageBrokingConsumerSettings"))
                .AddSingleton<IConsumer, Consumer>()
                .AddSingleton<IHostedService, TConsumer>();
        }
    }
}