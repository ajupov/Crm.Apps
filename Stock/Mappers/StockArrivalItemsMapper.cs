using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Stock.Models;

namespace Crm.Apps.Stock.Mappers
{
    public static class StockArrivalItemsMapper
    {
        public static List<StockArrivalItem> Map(this List<StockArrivalItem> items, Guid arrivalId)
        {
            return items?
                .Select(l => Map(l, arrivalId))
                .ToList();
        }

        public static StockArrivalItem Map(this StockArrivalItem item, Guid arrivalId)
        {
            return new StockArrivalItem
            {
                Id = item.Id,
                StockArrivalId = arrivalId,
                ProductId = item.ProductId,
                Count = item.Count,

                // UniqueElementIds = item.UniqueElementIds
            };
        }
    }
}
