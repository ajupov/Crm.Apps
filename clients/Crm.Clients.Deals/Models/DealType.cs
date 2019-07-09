using System;

namespace Crm.Clients.Deals.Models
{
    public class DealType
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }
        
        public DateTime CreateDateTime { get; set; }
    }
}