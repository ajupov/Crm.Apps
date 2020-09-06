using System.Collections.Generic;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.V1.Responses
{
    public class ActivityCommentGetPagedListResponse
    {
        public List<ActivityComment> Comments { get; set; }
    }
}
