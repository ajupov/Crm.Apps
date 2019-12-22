using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.RequestParameters;
using Crm.Apps.Areas.Activities.Services;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Areas.Activities.Consumers
{
    public class ActivityCommentsConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IActivityCommentsService _activityCommentsService;

        public ActivityCommentsConsumer(IConsumer consumer, IActivityCommentsService activityCommentsService)
        {
            _consumer = consumer;
            _activityCommentsService = activityCommentsService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("ActivityComments", ActionAsync);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken ct)
        {
            _consumer.UnConsume();

            return Task.CompletedTask;
        }

        private Task ActionAsync(Message message, CancellationToken ct)
        {
            return message.Type switch
            {
                "Create" => CreateAsync(message, ct),
                _ => Task.CompletedTask
            };
        }

        private Task CreateAsync(Message message, CancellationToken ct)
        {
            var request = message.Data.FromJsonString<ActivityCommentCreateRequest>();

            return _activityCommentsService.CreateAsync(message.UserId, request, ct);
        }
    }
}