using System;

namespace Crm.Apps.Deals.Models
{
    public class DealStatus
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }
        
        public string Name { get; set; }

        public bool IsFinish { get; set; }

        public bool IsDeleted { get; set; }
        
        public DateTime CreateDateTime { get; set; }
    }
}