using System;
using System.Collections.Generic;
using Crm.Apps.Suppliers.Models;

namespace Crm.Apps.Suppliers.V1.Responses
{
    public class SupplierGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<Supplier> Suppliers { get; set; }
    }
}
