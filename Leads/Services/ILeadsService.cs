using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.V1.Requests;
using Crm.Apps.Leads.V1.Responses;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadsService
    {
        Task<Lead> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<Lead>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<LeadGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            LeadGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Lead lead, CancellationToken ct);

        Task UpdateAsync(Guid userId, Lead oldLead, Lead newLead, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
