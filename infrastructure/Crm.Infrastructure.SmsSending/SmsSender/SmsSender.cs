using System.Threading.Tasks;
using Crm.Infrastructure.SmsSending.Settings;
using MainSMS;
using Microsoft.Extensions.Options;

namespace Crm.Infrastructure.SmsSending.SmsSender
{
    public class SmsSender : ISmsSender
    {
        private readonly SmsSendingSettings _settings;

        public SmsSender(IOptions<SmsSendingSettings> options)
        {
            _settings = options.Value;
        }

        public Task SendAsync(string phoneNumber, string message)
        {
            var client = new MainSmsClient(_settings.AccountName, _settings.ApiKey);

            return client.SendAsync(phoneNumber, message);
        }
    }
}