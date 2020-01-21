using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Leads.Models;
using Crm.Apps.v1.Clients.Leads.RequestParameters;

namespace Crm.Apps.v1.Clients.Leads.Clients
{
    public interface ILeadSourcesClient
    {
        Task<LeadSource> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<LeadSource>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<LeadSource>> GetPagedListAsync(
            LeadSourceGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(LeadSource source, CancellationToken ct = default);

        Task UpdateAsync(LeadSource source, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}