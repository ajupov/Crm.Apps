using System;

namespace Identity.Identities.Models
{
    public class Identity
    {
        public Identity(
            Guid userId,
            IdentityType type,
            string key,
            bool isPrimary)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Type = type;
            Key = key;
            IsPrimary = isPrimary;
            IsVerified = false;
            CreateDateTime = DateTime.UtcNow;
        }

        public Guid Id { get; }

        public Guid UserId { get; }

        public IdentityType Type { get; }

        public string Key { get; set; }

        public string PasswordHash { get; set; }

        public bool IsPrimary { get; set; }

        public bool IsVerified { get; set; }

        public DateTime CreateDateTime { get; }
    }
}