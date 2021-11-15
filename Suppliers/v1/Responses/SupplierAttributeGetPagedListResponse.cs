using System;
using System.Collections.Generic;
using Crm.Apps.Suppliers.Models;

namespace Crm.Apps.Suppliers.V1.Responses
{
    public class SupplierAttributeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<SupplierAttribute> Attributes { get; set; }
    }
}
