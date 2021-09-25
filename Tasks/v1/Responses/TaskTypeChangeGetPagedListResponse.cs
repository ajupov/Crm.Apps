using System.Collections.Generic;
using Crm.Apps.Tasks.Models;

namespace Crm.Apps.Tasks.V1.Responses
{
    public class TaskTypeChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<TaskTypeChange> Changes { get; set; }
    }
}
