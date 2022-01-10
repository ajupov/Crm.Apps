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
                   (request.SupplierIds == null || !request.SupplierIds.Any() ||
                    request.SupplierIds.Any(x => SupplierIdsPredicate(consumption, x))) &&
                   (request.OrderIds == null || !request.OrderIds.Any() ||
                    request.OrderIds.Any(x => OrderIdsPredicate(consumption, x))) &&
                   (request.InventoryIds == null || !request.InventoryIds.Any() ||
                    request.InventoryIds.Any(x => InventoryIdsPredicate(consumption, x))) &&
                   (request.ItemsRoomIds == null || !request.ItemsRoomIds.Any() ||
                    request.ItemsRoomIds.Any(x => ItemsRoomIdsPredicate(consumption, x))) &&
                   (request.ItemsProductIds == null || !request.ItemsProductIds.Any() ||
                    request.ItemsProductIds.Any(x => ItemsProductIdsPredicate(consumption, x)));
        }

        private static bool TypesPredicate(StockConsumption consumption, StockConsumptionType type)
        {
            return consumption.Type == type;
        }

        private static bool SupplierIdsPredicate(StockConsumption consumption, Guid id)
        {
            return consumption.SupplierId == id;
        }

        private static bool OrderIdsPredicate(StockConsumption consumption, Guid id)
        {
            return consumption.OrderId == id;
        }

        private static bool InventoryIdsPredicate(StockConsumption consumption, Guid id)
        {
            return consumption.InventoryId == id;
        }

        private static bool CreateUserIdsPredicate(StockConsumption consumption, Guid id)
        {
            return consumption.CreateUserId == id;
        }

        private static bool ItemsRoomIdsPredicate(StockConsumption consumption, Guid id)
        {
            return consumption.Items == null || !consumption.Items.Any() ||
                   consumption.Items.Any(x => x.RoomId == id);
        }

        private static bool ItemsProductIdsPredicate(StockConsumption consumption, Guid id)
        {
            return consumption.Items == null || !consumption.Items.Any() ||
                   consumption.Items.Any(x => x.ProductId == id);
        }
    }
}
