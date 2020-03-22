using System.Collections.Generic;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.v1.Responses
{
    public class ActivityAttributeChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<ActivityAttributeChange> Changes { get; set; }
    }
}