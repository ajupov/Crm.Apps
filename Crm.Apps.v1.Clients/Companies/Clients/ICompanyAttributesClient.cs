using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Companies.Models;
using Crm.Apps.v1.Clients.Companies.RequestParameters;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.v1.Clients.Companies.Clients
{
    public interface ICompanyAttributesClient
    {
        Task<Dictionary<string, AttributeType>> GetTypesAsync(string accessToken, CancellationToken ct = default);

        Task<CompanyAttribute> GetAsync(string accessToken, Guid id, CancellationToken ct = default);

        Task<List<CompanyAttribute>> GetListAsync(
            string accessToken,
            IEnumerable<Guid> ids,
            CancellationToken ct = default);

        Task<List<CompanyAttribute>> GetPagedListAsync(
            string accessToken,
            CompanyAttributeGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(string accessToken, CompanyAttribute attribute, CancellationToken ct = default);

        Task UpdateAsync(string accessToken, CompanyAttribute attribute, CancellationToken ct = default);

        Task DeleteAsync(string accessToken, IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(string accessToken, IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}