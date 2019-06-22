﻿using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Services;
using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Models;
using Crm.Utils.Json;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Leads.Consumers
{
    public class LeadCommentsConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly ILeadCommentsService _leadCommentsService;

        public LeadCommentsConsumer(IConsumer consumer, ILeadCommentsService leadCommentsService)
        {
            _consumer = consumer;
            _leadCommentsService = leadCommentsService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("LeadComments", ActionAsync);

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
            var comment = message.Data.FromJsonString<LeadComment>();

            return _leadCommentsService.CreateAsync(message.UserId, comment, ct);
        }
    }
}