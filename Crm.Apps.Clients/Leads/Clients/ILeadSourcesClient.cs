using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Leads.Models;

namespace Crm.Apps.Clients.Leads.Clients
{
    public interface ILeadSourcesClient
    {
        Task<LeadSource> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<LeadSource>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<LeadSource>> GetPagedListAsync(Guid? accountId = default, string name = default,
            bool? isDeleted = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(LeadSource source, CancellationToken ct = default);

        Task UpdateAsync(LeadSource source, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}