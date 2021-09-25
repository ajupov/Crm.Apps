using System;
using System.Collections.Generic;
using Crm.Apps.Tasks.Models;

namespace Crm.Apps.Tasks.V1.Responses
{
    public class TaskGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<Task> Tasks { get; set; }
    }
}
