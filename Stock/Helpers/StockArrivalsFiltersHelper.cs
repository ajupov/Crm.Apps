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
                   (request.ItemsRoomIds == null || !request.ItemsRoomIds.Any() ||
                    request.ItemsRoomIds.Any(x => ItemsRoomIdsPredicate(arrival, x))) &&
                   (request.ItemsProductIds == null || !request.ItemsProductIds.Any() ||
                    request.ItemsProductIds.Any(x => ItemsProductIdsPredicate(arrival, x)));
        }

        private static bool TypesPredicate(StockArrival arrival, StockArrivalType type)
        {
            return arrival.Type == type;
        }

        private static bool OrderIdsPredicate(StockArrival arrival, Guid id)
        {
            return arrival.OrderId == id;
        }

        private static bool CreateUserIdsPredicate(StockArrival arrival, Guid id)
        {
            return arrival.CreateUserId == id;
        }

        private static bool ItemsRoomIdsPredicate(StockArrival arrival, Guid id)
        {
            return arrival.Items == null || !arrival.Items.Any() ||
                   arrival.Items.Any(x => x.RoomId == id);
        }

        private static bool ItemsProductIdsPredicate(StockArrival arrival, Guid id)
        {
            return arrival.Items == null || !arrival.Items.Any() ||
                   arrival.Items.Any(x => x.ProductId == id);
        }
    }
}
