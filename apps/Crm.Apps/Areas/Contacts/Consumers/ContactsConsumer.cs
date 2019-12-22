using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Contacts.Models;
using Crm.Apps.Areas.Contacts.Services;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Areas.Contacts.Consumers
{
    public class ContactsConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IContactsService _contactsService;

        public ContactsConsumer(IConsumer consumer, IContactsService contactsService)
        {
            _consumer = consumer;
            _contactsService = contactsService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("Contacts", ActionAsync);

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
            var contact = message.Data.FromJsonString<Contact>();

            return _contactsService.CreateAsync(message.UserId, contact, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newContact = message.Data.FromJsonString<Contact>();
            if (newContact.Id.IsEmpty())
            {
                return;
            }

            var oldContact = await _contactsService.GetAsync(newContact.Id, ct);
            if (oldContact == null)
            {
                return;
            }

            await _contactsService.UpdateAsync(message.UserId, oldContact, newContact, ct);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _contactsService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _contactsService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}