using System;

namespace Crm.Apps.Clients.Activities.RequestParameters
{
    public class ActivityAttributeUpdateRequest
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public AttributeType Type { get; set; }

        public string Key { get; set; }

        public bool IsDeleted { get; set; }
    }
}