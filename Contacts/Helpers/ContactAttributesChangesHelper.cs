using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Contacts.Models;

namespace Crm.Apps.Contacts.Helpers
{
    public static class ContactAttributesChangesHelper
    {
        public static ContactAttributeChange CreateWithLog(
            this ContactAttribute attribute,
            Guid userId,
            Action<ContactAttribute> action)
        {
            action(attribute);

            return new ContactAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = attribute.ToJsonString()
            };
        }

        public static ContactAttributeChange UpdateWithLog(
            this ContactAttribute attribute,
            Guid userId,
            Action<ContactAttribute> action)
        {
            var oldValueJson = attribute.ToJsonString();

            action(attribute);

            return new ContactAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = attribute.ToJsonString()
            };
        }
    }
}
