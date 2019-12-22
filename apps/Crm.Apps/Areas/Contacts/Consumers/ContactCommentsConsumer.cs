using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Contacts.Models;
using Crm.Apps.Areas.Contacts.Services;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Areas.Contacts.Consumers
{
    public class ContactCommentsConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IContactCommentsService _contactCommentsService;

        public ContactCommentsConsumer(IConsumer consumer, IContactCommentsService contactCommentsService)
        {
            _consumer = consumer;
            _contactCommentsService = contactCommentsService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("ContactComments", ActionAsync);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken ct)
        {
            _consumer.UnConsume();

            return Task.CompletedTask;
        }

        private Task ActionAsync(Message message, CancellationToken ct)
        {
            switch (message.Type)
            {
                case "Create":
                    return CreateAsync(message, ct);
                default:
                    return Task.CompletedTask;
            }
        }

        private Task CreateAsync(Message message, CancellationToken ct)
        {
            var comment = message.Data.FromJsonString<ContactComment>();

            return _contactCommentsService.CreateAsync(message.UserId, comment, ct);
        }
    }
}