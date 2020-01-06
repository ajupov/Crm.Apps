using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Companies.Models;
using Crm.Apps.Clients.Companies.RequestParameters;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Clients.Companies.Clients
{
    public interface ICompanyAttributesClient
    {
        Task<Dictionary<string, AttributeType>> GetTypesAsync(CancellationToken ct = default);

        Task<CompanyAttribute> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<CompanyAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<CompanyAttribute>> GetPagedListAsync(
            CompanyAttributeGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(CompanyAttribute attribute, CancellationToken ct = default);

        Task UpdateAsync(CompanyAttribute attribute, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}