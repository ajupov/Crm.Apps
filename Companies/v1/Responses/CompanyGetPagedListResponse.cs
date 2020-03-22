using System;
using System.Collections.Generic;
using Crm.Apps.Companies.Models;

namespace Crm.Apps.Companies.v1.Responses
{
    public class CompanyGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<Company> Companies { get; set; }
    }
}