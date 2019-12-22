using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Services;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Areas.Deals.Consumers
{
    public class DealCommentsConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IDealCommentsService _dealCommentsService;

        public DealCommentsConsumer(IConsumer consumer, IDealCommentsService dealCommentsService)
        {
            _consumer = consumer;
            _dealCommentsService = dealCommentsService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("DealComments", ActionAsync);

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
            var comment = message.Data.FromJsonString<DealComment>();

            return _dealCommentsService.CreateAsync(message.UserId, comment, ct);
        }
    }
}