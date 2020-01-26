using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.v1.Models;
using Crm.Apps.Leads.v1.RequestParameters;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadCommentsService
    {
        Task<List<LeadComment>> GetPagedListAsync(
            LeadCommentGetPagedListRequestParameter request,
            CancellationToken ct);

        Task CreateAsync(Guid userId, LeadComment comment, CancellationToken ct);
    }
}