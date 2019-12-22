using System;
using Crm.Apps.Areas.Deals.Models;

namespace Crm.Apps.Areas.Deals.Helpers
{
    public static class DealsChangesHelper
    {
        public static DealChange CreateWithLog(this Deal deal, Guid dealId, Action<Deal> action)
        {
            action(deal);

            return new DealChange
            {
                DealId = deal.Id,
                ChangerUserId = dealId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = deal.ToJsonString()
            };
        }

        public static DealChange UpdateWithLog(this Deal deal, Guid dealId, Action<Deal> action)
        {
            var oldValueJson = deal.ToJsonString();

            action(deal);

            return new DealChange
            {
                DealId = deal.Id,
                ChangerUserId = dealId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = deal.ToJsonString()
            };
        }
    }
}