using System.Collections.Generic;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.v1.Responses
{
    public class ActivityChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<ActivityChange> Changes { get; set; }
    }
}