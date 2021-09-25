using System.Collections.Generic;
using Crm.Apps.Tasks.Models;

namespace Crm.Apps.Tasks.V1.Responses
{
    public class TaskStatusChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<TaskStatusChange> Changes { get; set; }
    }
}
