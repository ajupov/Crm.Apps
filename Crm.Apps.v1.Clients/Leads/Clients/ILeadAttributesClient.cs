using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Leads.Models;
using Crm.Apps.v1.Clients.Leads.RequestParameters;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.v1.Clients.Leads.Clients
{
    public interface ILeadAttributesClient
    {
        Task<Dictionary<string, AttributeType>> GetTypesAsync(string accessToken, CancellationToken ct = default);

        Task<LeadAttribute> GetAsync(string accessToken, Guid id, CancellationToken ct = default);

        Task<List<LeadAttribute>> GetListAsync(string accessToken, IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<LeadAttribute>> GetPagedListAsync(
            string accessToken,
            LeadAttributeGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(string accessToken, LeadAttribute attribute, CancellationToken ct = default);

        Task UpdateAsync(string accessToken, LeadAttribute attribute, CancellationToken ct = default);

        Task DeleteAsync(string accessToken, IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(string accessToken, IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}