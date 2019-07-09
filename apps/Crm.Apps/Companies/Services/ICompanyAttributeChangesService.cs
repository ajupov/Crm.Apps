﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Parameters;

namespace Crm.Apps.Companies.Services
{
    public interface ICompanyAttributeChangesService
    {
        Task<List<CompanyAttributeChange>> GetPagedListAsync(CompanyAttributeChangeGetPagedListParameter parameter,
            CancellationToken ct);
    }
}