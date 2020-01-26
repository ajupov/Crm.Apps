using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Contacts.v1.Models;

namespace Crm.Apps.Contacts.Helpers
{
    public static class ContactsChangesHelper
    {
        public static ContactChange CreateWithLog(this Contact contact, Guid userId, Action<Contact> action)
        {
            action(contact);

            return new ContactChange
            {
                ContactId = contact.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = contact.ToJsonString()
            };
        }

        public static ContactChange UpdateWithLog(this Contact contact, Guid userId, Action<Contact> action)
        {
            var oldValueJson = contact.ToJsonString();

            action(contact);

            return new ContactChange
            {
                ContactId = contact.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = contact.ToJsonString()
            };
        }
    }
}