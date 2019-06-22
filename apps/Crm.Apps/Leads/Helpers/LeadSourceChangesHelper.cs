using System;
using Crm.Apps.Leads.Models;
using Crm.Utils.Json;

namespace Crm.Apps.Leads.Helpers
{
    public static class LeadSourceChangesHelper
    {
        public static LeadSourceChange WithCreateLog(this LeadSource status, Guid userId, Action<LeadSource> action)
        {
            action(status);

            return new LeadSourceChange
            {
                SourceId = status.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = status.ToJsonString()
            };
        }

        public static LeadSourceChange WithUpdateLog(this LeadSource status, Guid userId, Action<LeadSource> action)
        {
            var oldValueJson = status.ToJsonString();

            action(status);

            return new LeadSourceChange
            {
                SourceId = status.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = status.ToJsonString()
            };
        }
    }
}