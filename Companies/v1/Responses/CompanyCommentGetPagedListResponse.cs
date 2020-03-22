using System.Collections.Generic;
using Crm.Apps.Companies.Models;

namespace Crm.Apps.Companies.v1.Responses
{
    public class CompanyCommentGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<CompanyComment> Comments { get; set; }
    }
}