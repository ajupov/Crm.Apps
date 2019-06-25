using System;
using Crm.Apps.Leads.Models;
using Crm.Utils.Json;

namespace Crm.Apps.Leads.Helpers
{
    public static class LeadsChangesHelper
    {
        public static LeadChange CreateWithLog(this Lead product, Guid productId, Action<Lead> action)
        {
            action(product);

            return new LeadChange
            {
                LeadId = product.Id,
                ChangerUserId = productId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = product.ToJsonString()
            };
        }

        public static LeadChange UpdateWithLog(this Lead product, Guid productId, Action<Lead> action)
        {
            var oldValueJson = product.ToJsonString();

            action(product);

            return new LeadChange
            {
                LeadId = product.Id,
                ChangerUserId = productId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = product.ToJsonString()
            };
        }
    }
}