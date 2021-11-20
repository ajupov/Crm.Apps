using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Stock.Models;

namespace Crm.Apps.Stock.Helpers
{
    public static class StockBalancesChangesHelper
    {
        public static StockBalanceChange CreateWithLog(
            this StockBalance balance,
            Guid userId,
            Action<StockBalance> action)
        {
            action(balance);

            return new StockBalanceChange
            {
                StockBalanceId = balance.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = balance.ToJsonString()
            };
        }

        public static StockBalanceChange UpdateWithLog(
            this StockBalance balance,
            Guid userId,
            Action<StockBalance> action)
        {
            var oldValueJson = balance.ToJsonString();

            action(balance);

            return new StockBalanceChange
            {
                StockBalanceId = balance.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = balance.ToJsonString()
            };
        }
    }
}
