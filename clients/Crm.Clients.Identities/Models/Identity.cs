using System;

namespace Crm.Clients.Identities.Models
{
    public class Identity
    {
        public Identity(Guid userId,
            IdentityType type,
            string key,
            bool isPrimary,
            bool isVerified)
        {
            Id = Guid.Empty;
            UserId = userId;
            Type = type;
            Key = key;
            IsPrimary = isPrimary;
            IsVerified = isVerified;
            CreateDateTime = DateTime.UtcNow;
        }

        public Guid Id { get; }

        public Guid UserId { get; set; }

        public IdentityType Type { get; set; }

        public string Key { get; set; }

        public bool IsPrimary { get; set; }

        public bool IsVerified { get; set; }

        public DateTime CreateDateTime { get; }
    }
}