using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Leads.Models;

namespace Crm.Clients.Leads.Clients
{
    public interface ILeadCommentsClient
    {
        Task<List<LeadSource>> GetPagedListAsync(Guid? leadId = default, Guid? commentatorUserId = default,
            string value = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(LeadComment comment, CancellationToken ct = default);
    }
}