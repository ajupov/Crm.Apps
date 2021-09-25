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
        public static bool FilterByAdditional(this Order product, OrderGetPagedListRequest request)
        {
            return (request.TypeIds == null || !request.TypeIds.Any() ||
                    request.TypeIds.Any(x => TypeIdsPredicate(product, x))) &&
                   (request.StatusIds == null || !request.StatusIds.Any() ||
                    request.StatusIds.Any(x => StatusIdsPredicate(product, x))) &&
                   (request.CreateUserIds == null || !request.CreateUserIds.Any() ||
                    request.CreateUserIds.Any(x => CreateUserIdsPredicate(product, x))) &&
                   (request.ResponsibleUserIds == null || !request.ResponsibleUserIds.Any() ||
                    request.ResponsibleUserIds.Any(x => ResponsibleUserIdsPredicate(product, x))) &&
                   (request.CustomerIds == null || !request.CustomerIds.Any() ||
                    request.CustomerIds.Any(x => CustomerIdsPredicate(product, x))) &&
                   (request.Attributes == null || !request.Attributes.Any() ||
                    (request.AllAttributes is false
                        ? request.Attributes.Any(x => AttributePredicate(product, x))
                        : request.Attributes.All(x => AttributePredicate(product, x)))) &&
                   (request.ItemsProductIds == null || !request.ItemsProductIds.Any() ||
                    request.ItemsProductIds.Any(x => PositionsProductIdsPredicate(product, x)));
        }

        private static bool TypeIdsPredicate(Order product, Guid id)
        {
            return product.TypeId == id;
        }

        private static bool StatusIdsPredicate(Order product, Guid id)
        {
            return product.StatusId == id;
        }

        private static bool CreateUserIdsPredicate(Order product, Guid id)
        {
            return product.CreateUserId == id;
        }

        private static bool ResponsibleUserIdsPredicate(Order product, Guid id)
        {
            return product.ResponsibleUserId == id;
        }

        private static bool CustomerIdsPredicate(Order product, Guid id)
        {
            return product.CustomerId == id;
        }

        private static bool AttributePredicate(Order product, KeyValuePair<Guid, string> pair)
        {
            var (key, value) = pair;

            return product.AttributeLinks != null && product.AttributeLinks.Any(x =>
                x.OrderAttributeId == key && (value.IsEmpty() || x.Value == value));
        }

        private static bool PositionsProductIdsPredicate(Order product, Guid id)
        {
            return product.Items == null || !product.Items.Any() ||
                   product.Items.Any(x => x.ProductId == id);
        }
    }
}
