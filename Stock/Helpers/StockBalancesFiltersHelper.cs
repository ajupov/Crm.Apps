using System;
using System.Linq;
using Crm.Apps.Stock.Models;
using Crm.Apps.Stock.V1.Requests;

namespace Crm.Apps.Stock.Helpers
{
    public static class StockBalancesFiltersHelper
    {
        public static bool FilterByAdditional(this StockBalance balance, StockBalanceGetPagedListRequest request)
        {
            return (request.CreateUserIds == null || !request.CreateUserIds.Any() ||
                    request.CreateUserIds.Any(x => CreateUserIdsPredicate(balance, x))) &&
                   (request.RoomIds == null || !request.RoomIds.Any() ||
                    request.RoomIds.Any(x => RoomIdsPredicate(balance, x))) &&
                   (request.ProductIds == null || !request.ProductIds.Any() ||
                    request.ProductIds.Any(x => ProductIdsPredicate(balance, x)));
        }

        private static bool CreateUserIdsPredicate(StockBalance balance, Guid id)
        {
            return balance.CreateUserId == id;
        }

        private static bool RoomIdsPredicate(StockBalance balance, Guid id)
        {
            return balance.RoomId == id;
        }

        private static bool ProductIdsPredicate(StockBalance balance, Guid id)
        {
            return balance.ProductId == id;
        }
    }
}
