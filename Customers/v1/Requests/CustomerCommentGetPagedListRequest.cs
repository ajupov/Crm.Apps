using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Customers.V1.Requests
{
    public class CustomerCommentGetPagedListRequest
    {
        [Required]
        public Guid CustomerId { get; set; }

        public DateTime? BeforeCreateDateTime { get; set; }

        public DateTime? AfterCreateDateTime { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; } = "CreateDateTime";

        public string OrderBy { get; set; } = "desc";
    }
}
