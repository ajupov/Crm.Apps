using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Tasks.Models;

namespace Crm.Apps.Tasks.Mappers
{
    public static class TaskAttributeLinksMapper
    {
        public static List<TaskAttributeLink> Map(this List<TaskAttributeLink> links, Guid taskId)
        {
            return links?
                .Select(l => Map(l, taskId))
                .ToList();
        }

        public static TaskAttributeLink Map(this TaskAttributeLink link, Guid taskId)
        {
            return new TaskAttributeLink
            {
                Id = link.Id,
                TaskId = taskId,
                TaskAttributeId = link.TaskAttributeId,
                Value = link.Value
            };
        }
    }
}
