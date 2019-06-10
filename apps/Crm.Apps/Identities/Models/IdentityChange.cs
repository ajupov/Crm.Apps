using System;

namespace Crm.Apps.Identities.Models
{
    public class IdentityChange
    {
        public Guid Id { get; set; }

        public Guid ChangerUserId { get; set; }

        public Guid IdentityId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string OldValueJson { get; set; }

        public string NewValueJson { get; set; }
    }
}