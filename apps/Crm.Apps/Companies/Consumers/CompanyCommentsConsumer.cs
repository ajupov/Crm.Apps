﻿using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Services;
using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Models;
using Crm.Utils.Json;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Companies.Consumers
{
    public class CompanyCommentsConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly ICompanyCommentsService _companiesCommentsService;

        public CompanyCommentsConsumer(IConsumer consumer, ICompanyCommentsService companiesCommentsService)
        {
            _consumer = consumer;
            _companiesCommentsService = companiesCommentsService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("CompanyComments", ActionAsync);

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
            var comment = message.Data.FromJsonString<CompanyComment>();

            return _companiesCommentsService.CreateAsync(message.UserId, comment, ct);
        }
    }
}