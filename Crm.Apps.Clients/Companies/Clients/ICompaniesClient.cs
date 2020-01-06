using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Companies.Models;
using Crm.Apps.Clients.Companies.RequestParameters;

namespace Crm.Apps.Clients.Companies.Clients
{
    public interface ICompaniesClient
    {
        Task<Dictionary<string, CompanyType>> GetTypesAsync(CancellationToken ct = default);

        Task<Dictionary<string, CompanyIndustryType>> GetIndustryTypesAsync(CancellationToken ct = default);

        Task<Company> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<Company>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Company>> GetPagedListAsync(
            CompanyGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(Company company, CancellationToken ct = default);

        Task UpdateAsync(Company company, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}