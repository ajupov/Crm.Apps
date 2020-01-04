using System;

namespace Crm.Apps.Clients.Products.Models
{
    public class ProductStatus
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }
        
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
        
        public DateTime CreateDateTime { get; set; }
    }
}