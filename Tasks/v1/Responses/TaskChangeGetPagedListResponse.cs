using System.Collections.Generic;
using Crm.Apps.Tasks.Models;

namespace Crm.Apps.Tasks.V1.Responses
{
    public class TaskChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<TaskChange> Changes { get; set; }
    }
}
