using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Deals.v1.Models;

namespace Crm.Apps.Deals.Mappers
{
    public static class DealPositionsMapper
    {
        public static List<DealPosition> Map(this List<DealPosition> positions, Guid dealId)
        {
            return positions?
                .Select(l => Map(l, dealId))
                .ToList();
        }

        public static DealPosition Map(this DealPosition position, Guid dealId)
        {
            return new DealPosition
            {
                Id = position.Id,
                DealId = dealId,
                ProductId = position.ProductId,
                ProductName = position.ProductName,
                ProductVendorCode = position.ProductVendorCode,
                Price = position.Price,
                Count = position.Count
            };
        }
    }
}