using System;
using System.Collections.Generic;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.V1.Responses
{
    public class ActivityTypeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<ActivityType> Types { get; set; }
    }
}
