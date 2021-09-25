using System.Collections.Generic;
using Crm.Apps.Tasks.Models;

namespace Crm.Apps.Tasks.V1.Responses
{
    public class TaskCommentGetPagedListResponse
    {
        public bool HasCommentsBefore { get; set; }

        public List<TaskComment> Comments { get; set; }
    }
}
