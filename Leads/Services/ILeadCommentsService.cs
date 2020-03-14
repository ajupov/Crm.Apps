using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.v1.Requests;
using Crm.Apps.Leads.v1.Responses;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadCommentsService
    {
        Task<LeadCommentGetPagedListResponse> GetPagedListAsync(
            LeadCommentGetPagedListRequest request,
            CancellationToken ct);

        Task CreateAsync(Guid userId, LeadComment comment, CancellationToken ct);
    }
}