﻿using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Products.Models;

namespace Crm.Apps.Products.Helpers
{
    public static class ProductStatusChangesHelper
    {
        public static ProductStatusChange CreateWithLog(
            this ProductStatus status,
            Guid userId,
            Action<ProductStatus> action)
        {
            action(status);

            return new ProductStatusChange
            {
                StatusId = status.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = status.ToJsonString()
            };
        }

        public static ProductStatusChange UpdateWithLog(
            this ProductStatus status,
            Guid userId,
            Action<ProductStatus> action)
        {
            var oldValueJson = status.ToJsonString();

            action(status);

            return new ProductStatusChange
            {
                StatusId = status.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = status.ToJsonString()
            };
        }
    }
}
