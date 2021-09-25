using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Customers.Models;

namespace Crm.Apps.Customers.Helpers
{
    public static class CustomersChangesHelper
    {
        public static CustomerChange CreateWithLog(this Customer customer, Guid userId, Action<Customer> action)
        {
            action(customer);

            return new CustomerChange
            {
                CustomerId = customer.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = customer.ToJsonString()
            };
        }

        public static CustomerChange UpdateWithLog(this Customer customer, Guid userId, Action<Customer> action)
        {
            var oldValueJson = customer.ToJsonString();

            action(customer);

            return new CustomerChange
            {
                CustomerId = customer.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = customer.ToJsonString()
            };
        }
    }
}
