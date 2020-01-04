using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Leads.Models;

namespace Crm.Apps.Clients.Leads.Clients
{
    public interface ILeadSourceChangesClient
    {
        Task<List<LeadSourceChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? sourceId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default, int limit = 10,
            string sortBy = default, string orderBy = default, CancellationToken ct = default);
    }
}