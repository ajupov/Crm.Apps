using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Tasks.Models;

namespace Crm.Apps.Tasks.Helpers
{
    public static class TaskAttributesChangesHelper
    {
        public static TaskAttributeChange CreateWithLog(
            this TaskAttribute attribute,
            Guid userId,
            Action<TaskAttribute> action)
        {
            action(attribute);

            return new TaskAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = attribute.ToJsonString()
            };
        }

        public static TaskAttributeChange UpdateWithLog(
            this TaskAttribute attribute,
            Guid userId,
            Action<TaskAttribute> action)
        {
            var oldValueJson = attribute.ToJsonString();

            action(attribute);

            return new TaskAttributeChange
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
