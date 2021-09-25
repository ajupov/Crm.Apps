using System;
using System.Collections.Generic;
using Crm.Apps.Tasks.Models;

namespace Crm.Apps.Tasks.V1.Responses
{
    public class TaskTypeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<TaskType> Types { get; set; }
    }
}
