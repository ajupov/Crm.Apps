using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Customers.Models;

namespace Crm.Apps.Customers.Helpers
{
    public static class CustomerSourceChangesHelper
    {
        public static CustomerSourceChange CreateWithLog(this CustomerSource source, Guid userId, Action<CustomerSource> action)
        {
            action(source);

            return new CustomerSourceChange
            {
                SourceId = source.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = source.ToJsonString()
            };
        }

        public static CustomerSourceChange UpdateWithLog(this CustomerSource source, Guid userId, Action<CustomerSource> action)
        {
            var oldValueJson = source.ToJsonString();

            action(source);

            return new CustomerSourceChange
            {
                SourceId = source.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = source.ToJsonString()
            };
        }
    }
}
