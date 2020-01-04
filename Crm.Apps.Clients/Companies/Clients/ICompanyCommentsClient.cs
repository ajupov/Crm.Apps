using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Companies.Models;

namespace Crm.Clients.Companies.Clients
{
    public interface ICompanyCommentsClient
    {
        Task<List<CompanyComment>> GetPagedListAsync(Guid? companyId = default, Guid? commentatorUserId = default,
            string value = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default);

        Task CreateAsync(CompanyComment comment, CancellationToken ct = default);
    }
}