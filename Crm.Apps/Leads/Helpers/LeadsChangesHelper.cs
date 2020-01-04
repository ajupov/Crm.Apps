using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Leads.Models;

namespace Crm.Apps.Leads.Helpers
{
    public static class LeadsChangesHelper
    {
        public static LeadChange CreateWithLog(this Lead lead, Guid productId, Action<Lead> action)
        {
            action(lead);

            return new LeadChange
            {
                LeadId = lead.Id,
                ChangerUserId = productId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = lead.ToJsonString()
            };
        }

        public static LeadChange UpdateWithLog(this Lead lead, Guid productId, Action<Lead> action)
        {
            var oldValueJson = lead.ToJsonString();

            action(lead);

            return new LeadChange
            {
                LeadId = lead.Id,
                ChangerUserId = productId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = lead.ToJsonString()
            };
        }
    }
}