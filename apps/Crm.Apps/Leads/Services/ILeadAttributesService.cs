using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.RequestParameters;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadAttributesService
    {
        Task<LeadAttribute> GetAsync(Guid id, CancellationToken ct);

        Task<List<LeadAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<LeadAttribute>> GetPagedListAsync(LeadAttributeGetPagedListRequestParameter request, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, LeadAttribute attribute, CancellationToken ct);

        Task UpdateAsync(Guid userId, LeadAttribute oldAttribute, LeadAttribute newAttribute, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}