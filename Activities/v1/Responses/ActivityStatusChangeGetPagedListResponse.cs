using System.Collections.Generic;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.V1.Responses
{
    public class ActivityStatusChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<ActivityStatusChange> Changes { get; set; }
    }
}
