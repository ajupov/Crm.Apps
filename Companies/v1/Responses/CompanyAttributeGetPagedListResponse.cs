using System;
using System.Collections.Generic;
using Crm.Apps.Companies.Models;

namespace Crm.Apps.Companies.V1.Responses
{
    public class CompanyAttributeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<CompanyAttribute> Attributes { get; set; }
    }
}
