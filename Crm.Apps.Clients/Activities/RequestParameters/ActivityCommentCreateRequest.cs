using System;

namespace Crm.Clients.Activities.RequestParameters
{
    public class ActivityCommentCreateRequest
    {
        public Guid ActivityId { get; set; }

        public string Value { get; set; }
    }
}