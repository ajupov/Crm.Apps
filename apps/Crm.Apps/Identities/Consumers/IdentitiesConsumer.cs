using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Identities.Models;
using Crm.Apps.Identities.Services;
using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Models;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Identities.Consumers
{
    public class IdentitiesConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IdentitiesService _identitiesService;

        public IdentitiesConsumer(IConsumer consumer, IdentitiesService identitiesService)
        {
            _consumer = consumer;
            _identitiesService = identitiesService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("Identities", ActionAsync);

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
                case "Update":
                    return UpdateAsync(message, ct);
                case "Verify":
                    return VerifyAsync(message, ct);
                case "Unverify":
                    return UnverifyAsync(message, ct);
                case "SetAsPrimary":
                    return SetAsPrimaryAsync(message, ct);
                case "ResetAsPrimary":
                    return ResetAsPrimaryAsync(message, ct);
                default:
                    return Task.CompletedTask;
            }
        }

        private Task CreateAsync(Message message, CancellationToken ct)
        {
            var identity = message.Data.FromJsonString<Identity>();

            return _identitiesService.CreateAsync(message.UserId, identity, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newIdentity = message.Data.FromJsonString<Identity>();
            if (newIdentity.Id.IsEmpty())
            {
                return;
            }

            var oldIdentity = await _identitiesService.GetAsync(newIdentity.Id, ct);
            if (oldIdentity == null)
            {
                return;
            }

            await _identitiesService.UpdateAsync(message.UserId, oldIdentity, newIdentity, ct);
        }

        private Task VerifyAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _identitiesService.VerifyAsync(message.UserId, ids, ct);
        }

        private Task UnverifyAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _identitiesService.UnverifyAsync(message.UserId, ids, ct);
        }

        private Task SetAsPrimaryAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _identitiesService.SetAsPrimaryAsync(message.UserId, ids, ct);
        }

        private Task ResetAsPrimaryAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _identitiesService.ResetAsPrimaryAsync(message.UserId, ids, ct);
        }
    }
}