﻿using Crm.Infrastructure.SmsSending.Settings;
using Crm.Infrastructure.SmsSending.SmsSender;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Infrastructure.SmsSending
{
    public static class MailSendingExtensions
    {
        public static IServiceCollection ConfigureMailSending(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<SmsSendingSettings>(configuration.GetSection("SmsSendingSettings"))
                .AddSingleton<ISmsSender, SmsSender.SmsSender>()
                .BuildServiceProvider();

            return services;
        }
    }
}