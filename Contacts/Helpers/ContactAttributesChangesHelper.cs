using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Contacts.v1.Models;

namespace Crm.Apps.Contacts.Helpers
{
    public static class ContactAttributesChangesHelper
    {
        public static ContactAttributeChange WithCreateLog(
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

        public static ContactAttributeChange WithUpdateLog(
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