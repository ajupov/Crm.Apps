using System;
using System.Collections.Generic;
using Crm.Apps.Products.Models;

namespace Crm.Apps.Products.V1.Responses
{
    public class ProductAttributeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<ProductAttribute> Attributes { get; set; }
    }
}
