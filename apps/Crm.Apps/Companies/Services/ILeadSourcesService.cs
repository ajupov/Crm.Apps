using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Parameters;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadSourcesService
    {
        Task<LeadSource> GetAsync(Guid id, CancellationToken ct);

        Task<List<LeadSource>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<LeadSource>> GetPagedListAsync(LeadSourceGetPagedListParameter parameter, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, LeadSource source, CancellationToken ct);

        Task UpdateAsync(Guid userId, LeadSource oldSource, LeadSource newSource, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}