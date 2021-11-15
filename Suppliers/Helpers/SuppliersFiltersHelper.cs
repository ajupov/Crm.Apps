using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Suppliers.Models;
using Crm.Apps.Suppliers.V1.Requests;

namespace Crm.Apps.Suppliers.Helpers
{
    public static class SuppliersFiltersHelper
    {
        public static bool FilterByAdditional(this Supplier supplier, SupplierGetPagedListRequest request)
        {
            return (request.CreateUserIds == null || !request.CreateUserIds.Any() ||
                    request.CreateUserIds.Any(x => CreateUserIdsPredicate(supplier, x))) &&
                   (request.Attributes == null || !request.Attributes.Any() ||
                    (request.AllAttributes is false
                        ? request.Attributes.Any(x => AttributePredicate(supplier, x))
                        : request.Attributes.All(x => AttributePredicate(supplier, x))));
        }

        private static bool CreateUserIdsPredicate(Supplier supplier, Guid id)
        {
            return supplier.CreateUserId == id;
        }

        private static bool AttributePredicate(Supplier supplier, KeyValuePair<Guid, string> pair)
        {
            var (key, value) = pair;

            return supplier.AttributeLinks != null && supplier.AttributeLinks.Any(x =>
                x.SupplierAttributeId == key && (value.IsEmpty() || x.Value == value));
        }
    }
}
