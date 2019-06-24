using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Parameters;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadCommentsService
    {
        Task<List<LeadComment>> GetPagedListAsync(LeadCommentGetPagedListParameter parameter, CancellationToken ct);

        Task CreateAsync(Guid userId, LeadComment comment, CancellationToken ct);
    }
}