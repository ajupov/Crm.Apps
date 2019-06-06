using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Settings;
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
                .Configure<MessageBrokingConsumerSettings>(configuration.GetSection("MessageBrokingConsumerSettings"))
                .Configure<MessageBrokingProducerSettings>(configuration.GetSection("MessageBrokingProducerSettings"))
                .AddSingleton<IConsumer, Consumer>()
                .AddSingleton<IHostedService, TConsumer>();
        }
    }
}