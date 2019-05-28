using Crm.Infrastructure.MailSending.MailSender;
using Crm.Infrastructure.MailSending.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Infrastructure.MailSending
{
    public static class MailSendingExtensions
    {
        public static IServiceCollection ConfigureMailSending(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MailSendingSettings>(configuration.GetSection("MailSendingSettings"))
                .AddSingleton<IMailSender, MailSender.MailSender>()
                .BuildServiceProvider();

            return services;
        }
    }
}