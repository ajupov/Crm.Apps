using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.Guid;
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
            var isNew = position.Id.IsEmpty();

            return new DealPosition
            {
                Id = isNew ? Guid.NewGuid() : position.Id,
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