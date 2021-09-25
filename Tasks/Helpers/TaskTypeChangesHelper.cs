using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Tasks.Models;

namespace Crm.Apps.Tasks.Helpers
{
    public static class TaskTypeChangesHelper
    {
        public static TaskTypeChange CreateWithLog(this TaskType type, Guid userId, Action<TaskType> action)
        {
            action(type);

            return new TaskTypeChange
            {
                TypeId = type.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = type.ToJsonString()
            };
        }

        public static TaskTypeChange UpdateWithLog(this TaskType type, Guid userId, Action<TaskType> action)
        {
            var oldValueJson = type.ToJsonString();

            action(type);

            return new TaskTypeChange
            {
                TypeId = type.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = type.ToJsonString()
            };
        }
    }
}
