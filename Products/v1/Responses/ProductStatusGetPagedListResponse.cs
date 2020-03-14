using System;
using System.Collections.Generic;
using Crm.Apps.Products.v1.Models;

namespace Crm.Apps.Products.v1.Responses
{
    public class ProductStatusGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<ProductStatus> Statuses { get; set; }
    }
}