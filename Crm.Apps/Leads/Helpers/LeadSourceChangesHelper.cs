using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Leads.v1.Models;

namespace Crm.Apps.Leads.Helpers
{
    public static class LeadSourceChangesHelper
    {
        public static LeadSourceChange WithCreateLog(this LeadSource source, Guid userId, Action<LeadSource> action)
        {
            action(source);

            return new LeadSourceChange
            {
                SourceId = source.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = source.ToJsonString()
            };
        }

        public static LeadSourceChange WithUpdateLog(this LeadSource source, Guid userId, Action<LeadSource> action)
        {
            var oldValueJson = source.ToJsonString();

            action(source);

            return new LeadSourceChange
            {
                SourceId = source.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = source.ToJsonString()
            };
        }
    }
}