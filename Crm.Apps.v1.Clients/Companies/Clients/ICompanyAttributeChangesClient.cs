﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Companies.Models;
using Crm.Apps.v1.Clients.Companies.RequestParameters;

namespace Crm.Apps.v1.Clients.Companies.Clients
{
    public interface ICompanyAttributeChangesClient
    {
        Task<List<CompanyAttributeChange>> GetPagedListAsync(
            string accessToken,
            CompanyAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}