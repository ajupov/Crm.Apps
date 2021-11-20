using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Orders.Models;
using Crm.Apps.Orders.V1.Requests;

namespace Crm.Apps.Orders.Helpers
{
    public static class OrdersFiltersHelper
    {
        public static bool FilterByAdditional(this Order order, OrderGetPagedListRequest request)
        {
            return (request.TypeIds == null || !request.TypeIds.Any() ||
                    request.TypeIds.Any(x => TypeIdsPredicate(order, x))) &&
                   (request.StatusIds == null || !request.StatusIds.Any() ||
                    request.StatusIds.Any(x => StatusIdsPredicate(order, x))) &&
                   (request.CreateUserIds == null || !request.CreateUserIds.Any() ||
                    request.CreateUserIds.Any(x => CreateUserIdsPredicate(order, x))) &&
                   (request.ResponsibleUserIds == null || !request.ResponsibleUserIds.Any() ||
                    request.ResponsibleUserIds.Any(x => ResponsibleUserIdsPredicate(order, x))) &&
                   (request.CustomerIds == null || !request.CustomerIds.Any() ||
                    request.CustomerIds.Any(x => CustomerIdsPredicate(order, x))) &&
                   (request.Attributes == null || !request.Attributes.Any() ||
                    (request.AllAttributes is false
                        ? request.Attributes.Any(x => AttributePredicate(order, x))
                        : request.Attributes.All(x => AttributePredicate(order, x)))) &&
                   (request.ItemsProductIds == null || !request.ItemsProductIds.Any() ||
                    request.ItemsProductIds.Any(x => ItemsProductIdsPredicate(order, x)));
        }

        private static bool TypeIdsPredicate(Order order, Guid id)
        {
            return order.TypeId == id;
        }

        private static bool StatusIdsPredicate(Order order, Guid id)
        {
            return order.StatusId == id;
        }

        private static bool CreateUserIdsPredicate(Order order, Guid id)
        {
            return order.CreateUserId == id;
        }

        private static bool ResponsibleUserIdsPredicate(Order order, Guid id)
        {
            return order.ResponsibleUserId == id;
        }

        private static bool CustomerIdsPredicate(Order order, Guid id)
        {
            return order.CustomerId == id;
        }

        private static bool AttributePredicate(Order order, KeyValuePair<Guid, string> pair)
        {
            var (key, value) = pair;

            return order.AttributeLinks != null && order.AttributeLinks.Any(x =>
                x.OrderAttributeId == key && (value.IsEmpty() || x.Value == value));
        }

        private static bool ItemsProductIdsPredicate(Order order, Guid id)
        {
            return order.Items == null || !order.Items.Any() ||
                   order.Items.Any(x => x.ProductId == id);
        }
    }
}
