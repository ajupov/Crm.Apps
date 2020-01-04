using System;

namespace Crm.Apps.Products.Models
{
    public class ProductCategoryLink
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public Guid ProductCategoryId { get; set; }

        public DateTime CreateDateTime { get; set; }
        
        public DateTime? ModifyDateTime { get; set; }
    }
}