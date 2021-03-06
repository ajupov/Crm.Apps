﻿using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Companies.Models;

namespace Crm.Apps.Companies.Helpers
{
    public static class CompanyAttributesChangesHelper
    {
        public static CompanyAttributeChange CreateWithLog(
            this CompanyAttribute attribute,
            Guid userId,
            Action<CompanyAttribute> action)
        {
            action(attribute);

            return new CompanyAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = attribute.ToJsonString()
            };
        }

        public static CompanyAttributeChange UpdateWithLog(
            this CompanyAttribute attribute,
            Guid userId,
            Action<CompanyAttribute> action)
        {
            var oldValueJson = attribute.ToJsonString();

            action(attribute);

            return new CompanyAttributeChange
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
