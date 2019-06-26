using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Parameters;

namespace Crm.Apps.Companies.Services
{
    public interface ICompaniesService
    {
        Task<Company> GetAsync(Guid id, CancellationToken ct);

        Task<List<Company>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<Company>> GetPagedListAsync(CompanyGetPagedListParameter parameter, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Company company, CancellationToken ct);

        Task UpdateAsync(Guid userId, Company oldCompany, Company newCompany, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}