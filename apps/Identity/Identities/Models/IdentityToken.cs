using System;

namespace Identity.Identities.Models
{
    public class IdentityToken
    {
        public IdentityToken(
            Guid identityId,
            IdentityTokenType type,
            string value,
            DateTime expirationDateTime,
            string userAgent,
            string ipAddress)
        {
            Id = Guid.NewGuid();
            IdentityId = identityId;
            Type = type;
            Value = value;
            CreateDateTime = DateTime.UtcNow;
            ExpirationDateTime = expirationDateTime;
            UserAgent = userAgent;
            IpAddress = ipAddress;
        }

        public Guid Id { get; }

        public Guid IdentityId { get; }

        public IdentityTokenType Type { get; }

        public string Value { get; }

        public DateTime CreateDateTime { get; }

        public DateTime ExpirationDateTime { get; }

        public DateTime? UseDateTime { get; set; }

        public string UserAgent { get; }

        public string IpAddress { get; }
    }
}