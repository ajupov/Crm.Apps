using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Stock.Models;

namespace Crm.Apps.Stock.Mappers
{
    public static class StockConsumptionItemsMapper
    {
        public static List<StockConsumptionItem> Map(this List<StockConsumptionItem> items, Guid consumptionId)
        {
            return items?
                .Select(l => Map(l, consumptionId))
                .ToList();
        }

        public static StockConsumptionItem Map(this StockConsumptionItem item, Guid consumptionId)
        {
            return new StockConsumptionItem
            {
                Id = item.Id,
                StockConsumptionId = consumptionId,
                ProductId = item.ProductId,
                Count = item.Count,
                UniqueElementIds = item.UniqueElementIds
            };
        }
    }
}
