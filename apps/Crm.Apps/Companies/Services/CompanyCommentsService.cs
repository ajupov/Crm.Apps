using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.Helpers;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Parameters;
using Crm.Apps.Companies.Storages;
using Crm.Utils.Guid;
using Crm.Utils.String;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Companies.Services
{
    public class CompanyCommentsService : ICompanyCommentsService
    {
        private readonly CompaniesStorage _storage;

        public CompanyCommentsService(CompaniesStorage storage)
        {
            _storage = storage;
        }

        public Task<List<CompanyComment>> GetPagedListAsync(CompanyCommentGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.CompanyComments.Where(x =>
                    x.CompanyId == parameter.CompanyId &&
                    (parameter.CommentatorUserId.IsEmpty() || x.CommentatorUserId == parameter.CommentatorUserId) &&
                    (parameter.Value.IsEmpty() || EF.Functions.Like(x.Value, $"{parameter.Value}%")) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }

        public async Task CreateAsync(Guid userId, CompanyComment comment, CancellationToken ct)
        {
            var newComment = new CompanyComment
            {
                Id = Guid.NewGuid(),
                CompanyId = comment.CompanyId,
                CommentatorUserId = userId,
                Value = comment.Value,
                CreateDateTime = DateTime.UtcNow
            };

            await _storage.AddAsync(newComment, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}