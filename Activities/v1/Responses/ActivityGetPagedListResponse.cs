using System;
using System.Collections.Generic;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.v1.Responses
{
    public class ActivityGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<Activity> Activities { get; set; }
    }
}