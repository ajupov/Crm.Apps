using System.Collections.Generic;
using Crm.Apps.Companies.Models;

namespace Crm.Apps.Companies.V1.Responses
{
    public class CompanyChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<CompanyChange> Changes { get; set; }
    }
}
