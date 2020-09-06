using System.Collections.Generic;
using Crm.Apps.Companies.Models;

namespace Crm.Apps.Companies.V1.Responses
{
    public class CompanyCommentGetPagedListResponse
    {
        public List<CompanyComment> Comments { get; set; }
    }
}
