using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Suppliers.V1.Requests
{
    public class SupplierCommentGetPagedListRequest
    {
        [Required]
        public Guid SupplierId { get; set; }

        public DateTime? BeforeCreateDateTime { get; set; }

        public DateTime? AfterCreateDateTime { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; } = "CreateDateTime";

        public string OrderBy { get; set; } = "desc";
    }
}
