using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Storages;
using Crm.Apps.Leads.V1.Requests;
using Crm.Apps.Leads.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Leads.Services
{
    public class LeadCommentsService : ILeadCommentsService
    {
        private readonly LeadsStorage _storage;

        public LeadCommentsService(LeadsStorage storage)
        {
            _storage = storage;
        }

        public async Task<LeadCommentGetPagedListResponse> GetPagedListAsync(
            LeadCommentGetPagedListRequest request,
            CancellationToken ct)
        {
            var comments = _storage.LeadComments
                .Where(x =>
                    x.LeadId == request.LeadId &&
                    (!request.AfterCreateDateTime.HasValue || x.CreateDateTime > request.AfterCreateDateTime));

            return new LeadCommentGetPagedListResponse
            {
                Comments = await comments
                    .SortBy("CreateDateTime", "desc")
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }

        public async Task CreateAsync(Guid userId, LeadComment comment, CancellationToken ct)
        {
            var newComment = new LeadComment
            {
                Id = Guid.NewGuid(),
                LeadId = comment.LeadId,
                CommentatorUserId = userId,
                Value = comment.Value,
                CreateDateTime = DateTime.UtcNow
            };

            await _storage.AddAsync(newComment, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
