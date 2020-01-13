using System;

namespace Crm.Apps.RefreshTokens.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
        
        public DateTime CreateDateTime { get; set; }
        
        public DateTime ExpirationDateTime { get; set; }
    }
}