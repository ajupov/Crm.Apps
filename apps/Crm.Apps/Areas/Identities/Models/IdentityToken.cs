using System;

namespace Crm.Apps.Areas.Identities.Models
{
    public class IdentityToken
    {
        public Guid Id { get; set; }

        public Guid IdentityId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }
        
        public DateTime ExpirationDateTime { get; set; }
        
        public DateTime? UseDateTime { get; set; }
    }
}