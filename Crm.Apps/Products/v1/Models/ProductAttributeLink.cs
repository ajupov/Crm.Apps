using System;

namespace Crm.Apps.Products.v1.Models
{
    public class ProductAttributeLink
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public Guid ProductAttributeId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }
        
        public DateTime? ModifyDateTime { get; set; }
    }
}