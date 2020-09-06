using System;
using ServiceStack.DataAnnotations;

namespace Crm.Apps.Companies.V1.Requests
{
    public class CompanyCommentGetPagedListRequest
    {
        [Required]
        public Guid CompanyId { get; set; }

        public DateTime? AfterCreateDateTime { get; set; }

        public int Limit { get; set; } = 10;
    }
}
