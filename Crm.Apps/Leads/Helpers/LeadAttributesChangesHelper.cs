using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Leads.v1.Models;

namespace Crm.Apps.Leads.Helpers
{
    public static class LeadAttributesChangesHelper
    {
        public static LeadAttributeChange WithCreateLog(this LeadAttribute attribute, Guid userId,
            Action<LeadAttribute> action)
        {
            action(attribute);

            return new LeadAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = attribute.ToJsonString()
            };
        }

        public static LeadAttributeChange WithUpdateLog(this LeadAttribute attribute, Guid userId,
            Action<LeadAttribute> action)
        {
            var oldValueJson = attribute.ToJsonString();

            action(attribute);

            return new LeadAttributeChange
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