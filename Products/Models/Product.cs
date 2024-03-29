using System;
using System.Collections.Generic;
using Crm.Common.All.Validation.Attributes;

namespace Crm.Apps.Products.Models
{
    public class Product
    {
        [NonDefaultGuid]
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public Guid? ParentProductId { get; set; }

        public ProductType Type { get; set; }

        public Guid StatusId { get; set; }

        public string Name { get; set; }

        public string VendorCode { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public bool IsHidden { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }

        public ProductStatus Status { get; set; }

        public List<ProductAttributeLink> AttributeLinks { get; set; }

        public List<ProductCategoryLink> CategoryLinks { get; set; }
    }
}
