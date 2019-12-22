using System;

namespace Crm.Apps.Areas.Products.Models
{
    public class ProductCategoryLink
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public Guid ProductCategoryId { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}