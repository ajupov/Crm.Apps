using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Services;
using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Models;
using Crm.Utils.Json;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Areas.Users.Consumers
{
    public class UserGroupsConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IUserGroupsService _userGroupsService;

        public UserGroupsConsumer(IConsumer consumer, IUserGroupsService userGroupsService)
        {
            _consumer = consumer;
            _userGroupsService = userGroupsService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("UserGroups", ActionAsync);

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
                case "Delete":
                    return DeleteAsync(message, ct);
                case "Restore":
                    return RestoreAsync(message, ct);
                default:
                    return Task.CompletedTask;
            }
        }

        private Task CreateAsync(Message message, CancellationToken ct)
        {
            var userGroup = message.Data.FromJsonString<UserGroup>();
            if (userGroup.Id == Guid.Empty)
            {
                return Task.CompletedTask;
            }

            return _userGroupsService.CreateAsync(message.UserId, userGroup, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newUserGroup = message.Data.FromJsonString<UserGroup>();
            if (newUserGroup.Id == Guid.Empty)
            {
                return;
            }

            var oldUserGroup = await _userGroupsService.GetAsync(newUserGroup.Id, ct).ConfigureAwait(false);
            if (oldUserGroup == null)
            {
                return;
            }

            await _userGroupsService.UpdateAsync(message.UserId, oldUserGroup, newUserGroup, ct).ConfigureAwait(false);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<ICollection<Guid>>();
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return Task.CompletedTask;
            }

            return _userGroupsService.RestoreAsync(message.UserId, ids, ct);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<ICollection<Guid>>();
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return Task.CompletedTask;
            }

            return _userGroupsService.DeleteAsync(message.UserId, ids, ct);
        }
    }
}