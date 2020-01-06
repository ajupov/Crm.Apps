using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Leads.Models;
using Crm.Apps.Clients.Leads.RequestParameters;

namespace Crm.Apps.Clients.Leads.Clients
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