using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Stock.Models;

namespace Crm.Apps.Stock.Helpers
{
    public static class StockArrivalsChangesHelper
    {
        public static StockArrivalChange CreateWithLog(
            this StockArrival arrival,
            Guid userId,
            Action<StockArrival> action)
        {
            action(arrival);

            return new StockArrivalChange
            {
                StockArrivalId = arrival.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = arrival.ToJsonString()
            };
        }

        public static StockArrivalChange UpdateWithLog(
            this StockArrival arrival,
            Guid userId,
            Action<StockArrival> action)
        {
            var oldValueJson = arrival.ToJsonString();

            action(arrival);

            return new StockArrivalChange
            {
                StockArrivalId = arrival.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = arrival.ToJsonString()
            };
        }
    }
}
