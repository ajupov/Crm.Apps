using System;
using System.Collections.Generic;
using Crm.Apps.Products.v1.Models;

namespace Crm.Apps.Products.v1.Responses
{
    public class ProductCategoryGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<ProductCategory> Categories { get; set; }
    }
}