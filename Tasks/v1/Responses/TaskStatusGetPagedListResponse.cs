using System;
using System.Collections.Generic;
using Crm.Apps.Tasks.Models;

namespace Crm.Apps.Tasks.V1.Responses
{
    public class TaskStatusGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<TaskStatus> Statuses { get; set; }
    }
}
