using System.Collections.Generic;
using Crm.Apps.Companies.Models;

namespace Crm.Apps.Companies.V1.Responses
{
    public class CompanyCommentGetPagedListResponse
    {
        public bool HasCommentsBefore { get; set; }

        public List<CompanyComment> Comments { get; set; }
    }
}
