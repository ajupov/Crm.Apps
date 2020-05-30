using System.Collections.Generic;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.V1.Responses
{
    public class ActivityAttributeChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<ActivityAttributeChange> Changes { get; set; }
    }
}
