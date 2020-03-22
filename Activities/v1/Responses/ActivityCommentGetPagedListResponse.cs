using System.Collections.Generic;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.v1.Responses
{
    public class ActivityCommentGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<ActivityComment> Comments { get; set; }
    }
}