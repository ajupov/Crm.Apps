using System;

namespace Crm.Apps.Products.Models
{
    public class ProductCategoryChange
    {
        public Guid Id { get; set; }

        public Guid ChangerUserId { get; set; }

        public Guid CategoryId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string OldValueJson { get; set; }

        public string NewValueJson { get; set; }
    }
}