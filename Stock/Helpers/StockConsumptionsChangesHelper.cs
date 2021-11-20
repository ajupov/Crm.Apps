using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Stock.Models;

namespace Crm.Apps.Stock.Helpers
{
    public static class StockConsumptionsChangesHelper
    {
        public static StockConsumptionChange CreateWithLog(
            this StockConsumption consumption,
            Guid userId,
            Action<StockConsumption> action)
        {
            action(consumption);

            return new StockConsumptionChange
            {
                StockConsumptionId = consumption.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = consumption.ToJsonString()
            };
        }

        public static StockConsumptionChange UpdateWithLog(
            this StockConsumption consumption,
            Guid userId,
            Action<StockConsumption> action)
        {
            var oldValueJson = consumption.ToJsonString();

            action(consumption);

            return new StockConsumptionChange
            {
                StockConsumptionId = consumption.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = consumption.ToJsonString()
            };
        }
    }
}
