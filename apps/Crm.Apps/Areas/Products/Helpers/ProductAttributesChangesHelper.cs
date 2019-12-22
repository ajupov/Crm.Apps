using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Areas.Products.Models;

namespace Crm.Apps.Areas.Products.Helpers
{
    public static class ProductAttributesChangesHelper
    {
        public static ProductAttributeChange WithCreateLog(this ProductAttribute attribute, Guid userId,
            Action<ProductAttribute> action)
        {
            action(attribute);

            return new ProductAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = attribute.ToJsonString()
            };
        }

        public static ProductAttributeChange WithUpdateLog(this ProductAttribute attribute, Guid userId,
            Action<ProductAttribute> action)
        {
            var oldValueJson = attribute.ToJsonString();

            action(attribute);

            return new ProductAttributeChange
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