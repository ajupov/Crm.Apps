using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.RequestParameters;
using Crm.Apps.Leads.Storages;
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

        public Task<List<LeadComment>> GetPagedListAsync(LeadCommentGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _storage.LeadComments
                .AsNoTracking()
                .Where(x =>
                    x.LeadId == request.LeadId &&
                    (request.CommentatorUserId.IsEmpty() || x.CommentatorUserId == request.CommentatorUserId) &&
                    (request.Value.IsEmpty() || EF.Functions.Like(x.Value, $"{request.Value}%")) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
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