using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.RequestParameters;

namespace Crm.Apps.Areas.Leads.Services
{
    public interface ILeadsService
    {
        Task<Lead> GetAsync(Guid id, CancellationToken ct);

        Task<List<Lead>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<Lead>> GetPagedListAsync(LeadGetPagedListRequestParameter request, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Lead lead, CancellationToken ct);

        Task UpdateAsync(Guid userId, Lead oldLead, Lead newLead, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}