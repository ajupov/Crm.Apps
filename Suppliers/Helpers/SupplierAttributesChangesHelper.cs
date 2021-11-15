using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Suppliers.Models;

namespace Crm.Apps.Suppliers.Helpers
{
    public static class SupplierAttributesChangesHelper
    {
        public static SupplierAttributeChange CreateWithLog(
            this SupplierAttribute attribute,
            Guid userId,
            Action<SupplierAttribute> action)
        {
            action(attribute);

            return new SupplierAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = attribute.ToJsonString()
            };
        }

        public static SupplierAttributeChange UpdateWithLog(
            this SupplierAttribute attribute,
            Guid userId,
            Action<SupplierAttribute> action)
        {
            var oldValueJson = attribute.ToJsonString();

            action(attribute);

            return new SupplierAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = attribute.ToJsonString()
            };
        }
    }
}
