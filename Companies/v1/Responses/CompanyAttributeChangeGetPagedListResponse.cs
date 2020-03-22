using System.Collections.Generic;
using Crm.Apps.Companies.Models;

namespace Crm.Apps.Companies.v1.Responses
{
    public class CompanyAttributeChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<CompanyAttributeChange> Changes { get; set; }
    }
}