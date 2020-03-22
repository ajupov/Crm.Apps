using System.Collections.Generic;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.v1.Responses
{
    public class ActivityTypeChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<ActivityTypeChange> Changes { get; set; }
    }
}