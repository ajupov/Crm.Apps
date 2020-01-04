using System;

namespace Crm.Apps.Products.Models
{
    public class ProductStatus
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }
        
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
        
        public DateTime CreateDateTime { get; set; }
        
        public DateTime? ModifyDateTime { get; set; }
    }
}