using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.Parameters;

namespace Crm.Apps.Areas.Companies.Services
{
    public interface ICompanyAttributesService
    {
        Task<CompanyAttribute> GetAsync(Guid id, CancellationToken ct);

        Task<List<CompanyAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<CompanyAttribute>> GetPagedListAsync(
            CompanyAttributeGetPagedListParameter parameter,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, CompanyAttribute attribute, CancellationToken ct);

        Task UpdateAsync(
            Guid userId,
            CompanyAttribute oldAttribute,
            CompanyAttribute newAttribute,
            CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}