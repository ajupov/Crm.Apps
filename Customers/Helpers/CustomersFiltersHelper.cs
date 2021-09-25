using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Customers.Models;
using Crm.Apps.Customers.V1.Requests;

namespace Crm.Apps.Customers.Helpers
{
    public static class CustomersFiltersHelper
    {
        public static bool FilterByAdditional(this Customer customer, CustomerGetPagedListRequest request)
        {
            return (request.SourceIds == null || !request.SourceIds.Any() ||
                    request.SourceIds.Any(x => SourceIdsPredicate(customer, x))) &&
                   (request.CreateUserIds == null || !request.CreateUserIds.Any() ||
                    request.CreateUserIds.Any(x => CreateUserIdsPredicate(customer, x))) &&
                   (request.ResponsibleUserIds == null || !request.ResponsibleUserIds.Any() ||
                    request.ResponsibleUserIds.Any(x => ResponsibleUserIdsPredicate(customer, x))) &&
                   (request.Attributes == null || !request.Attributes.Any() ||
                    (request.AllAttributes is false
                        ? request.Attributes.Any(x => AttributePredicate(customer, x))
                        : request.Attributes.All(x => AttributePredicate(customer, x))));
        }

        private static bool SourceIdsPredicate(Customer customer, Guid id)
        {
            return customer.SourceId == id;
        }

        private static bool CreateUserIdsPredicate(Customer customer, Guid id)
        {
            return customer.CreateUserId == id;
        }

        private static bool ResponsibleUserIdsPredicate(Customer customer, Guid id)
        {
            return customer.ResponsibleUserId == id;
        }

        private static bool AttributePredicate(Customer customer, KeyValuePair<Guid, string> pair)
        {
            var (key, value) = pair;

            return customer.AttributeLinks != null && customer.AttributeLinks.Any(x =>
                       x.CustomerAttributeId == key && (value.IsEmpty() || x.Value == value));
        }
    }
}
