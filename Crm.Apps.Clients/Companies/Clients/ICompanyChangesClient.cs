using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Companies.Models;

namespace Crm.Clients.Companies.Clients
{
    public interface ICompanyChangesClient
    {
        Task<List<CompanyChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? companyId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default);
    }
}