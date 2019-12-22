using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Helpers;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.Parameters;
using Crm.Apps.Areas.Leads.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Leads.Services
{
    public class LeadCommentsService : ILeadCommentsService
    {
        private readonly LeadsStorage _storage;

        public LeadCommentsService(LeadsStorage storage)
        {
            _storage = storage;
        }

        public Task<List<LeadComment>> GetPagedListAsync(LeadCommentGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.LeadComments.Where(x =>
                    x.LeadId == parameter.LeadId &&
                    (parameter.CommentatorUserId.IsEmpty() || x.CommentatorUserId == parameter.CommentatorUserId) &&
                    (parameter.Value.IsEmpty() || EF.Functions.Like(x.Value, $"{parameter.Value}%")) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
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