using System.Collections.Generic;
using Crm.Apps.Tasks.Models;

namespace Crm.Apps.Tasks.V1.Responses
{
    public class TaskAttributeChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<TaskAttributeChange> Changes { get; set; }
    }
}
