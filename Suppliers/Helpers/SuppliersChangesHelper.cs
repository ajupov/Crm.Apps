using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Suppliers.Models;

namespace Crm.Apps.Suppliers.Helpers
{
    public static class SuppliersChangesHelper
    {
        public static SupplierChange CreateWithLog(this Supplier supplier, Guid userId, Action<Supplier> action)
        {
            action(supplier);

            return new SupplierChange
            {
                SupplierId = supplier.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = supplier.ToJsonString()
            };
        }

        public static SupplierChange UpdateWithLog(this Supplier supplier, Guid userId, Action<Supplier> action)
        {
            var oldValueJson = supplier.ToJsonString();

            action(supplier);

            return new SupplierChange
            {
                SupplierId = supplier.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = supplier.ToJsonString()
            };
        }
    }
}
