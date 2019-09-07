using System;

namespace Crm.Clients.Activities.RequestParameters
{
    public class ActivityTypeUpdateRequest
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}