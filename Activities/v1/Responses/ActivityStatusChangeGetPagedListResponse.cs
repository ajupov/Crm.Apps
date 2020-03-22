using System.Collections.Generic;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.v1.Responses
{
    public class ActivityStatusChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<ActivityStatusChange> Changes { get; set; }
    }
}