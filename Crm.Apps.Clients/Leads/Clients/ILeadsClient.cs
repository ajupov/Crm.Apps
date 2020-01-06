using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Leads.Models;
using Crm.Apps.Clients.Leads.RequestParameters;

namespace Crm.Apps.Clients.Leads.Clients
{
    public interface ILeadsClient
    {
        Task<Lead> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<Lead>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Lead>> GetPagedListAsync(LeadGetPagedListRequestParameter request, CancellationToken ct = default);

        Task<Guid> CreateAsync(Lead lead, CancellationToken ct = default);

        Task UpdateAsync(Lead lead, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}