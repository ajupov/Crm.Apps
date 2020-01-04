using System;

namespace Crm.Apps.Clients.Activities.RequestParameters
{
    public class ActivityTypeCreateRequest
    {
        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}