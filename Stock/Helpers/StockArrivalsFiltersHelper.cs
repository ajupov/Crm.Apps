using System;
using System.Linq;
using Crm.Apps.Stock.Models;
using Crm.Apps.Stock.V1.Requests;

namespace Crm.Apps.Stock.Helpers
{
    public static class StockArrivalsFiltersHelper
    {
        public static bool FilterByAdditional(this StockArrival arrival, StockArrivalGetPagedListRequest request)
        {
            return (request.Types == null || !request.Types.Any() ||
                    request.Types.Any(x => TypesPredicate(arrival, x))) &&
                   (request.CreateUserIds == null || !request.CreateUserIds.Any() ||
                    request.CreateUserIds.Any(x => CreateUserIdsPredicate(arrival, x))) &&
                   (request.OrderIds == null || !request.OrderIds.Any() ||
                    request.OrderIds.Any(x => OrderIdsPredicate(arrival, x))) &&
                   (request.ItemsProductIds == null || !request.ItemsProductIds.Any() ||
                    request.ItemsProductIds.Any(x => ItemsProductIdsPredicate(arrival, x)));
        }

        private static bool TypesPredicate(StockArrival product, StockArrivalType type)
        {
            return product.Type == type;
        }

        private static bool OrderIdsPredicate(StockArrival arrival, Guid id)
        {
            return arrival.OrderId == id;
        }

        private static bool CreateUserIdsPredicate(StockArrival product, Guid id)
        {
            return product.CreateUserId == id;
        }

        private static bool ItemsProductIdsPredicate(StockArrival product, Guid id)
        {
            return product.Items == null || !product.Items.Any() ||
                   product.Items.Any(x => x.ProductId == id);
        }
    }
}
