using System;
using System.Linq;
using Crm.Apps.Stock.Models;
using Crm.Apps.Stock.V1.Requests;

namespace Crm.Apps.Stock.Helpers
{
    public static class StockConsumptionsFiltersHelper
    {
        public static bool FilterByAdditional(
            this StockConsumption consumption,
            StockConsumptionGetPagedListRequest request)
        {
            return (request.Types == null || !request.Types.Any() ||
                    request.Types.Any(x => TypesPredicate(consumption, x))) &&
                   (request.CreateUserIds == null || !request.CreateUserIds.Any() ||
                    request.CreateUserIds.Any(x => CreateUserIdsPredicate(consumption, x))) &&
                   (request.OrderIds == null || !request.OrderIds.Any() ||
                    request.OrderIds.Any(x => OrderIdsPredicate(consumption, x))) &&
                   (request.ItemsProductIds == null || !request.ItemsProductIds.Any() ||
                    request.ItemsProductIds.Any(x => ItemsProductIdsPredicate(consumption, x)));
        }

        private static bool TypesPredicate(StockConsumption product, StockConsumptionType type)
        {
            return product.Type == type;
        }

        private static bool OrderIdsPredicate(StockConsumption consumption, Guid id)
        {
            return consumption.OrderId == id;
        }

        private static bool CreateUserIdsPredicate(StockConsumption product, Guid id)
        {
            return product.CreateUserId == id;
        }

        private static bool ItemsProductIdsPredicate(StockConsumption product, Guid id)
        {
            return product.Items == null || !product.Items.Any() ||
                   product.Items.Any(x => x.ProductId == id);
        }
    }
}
