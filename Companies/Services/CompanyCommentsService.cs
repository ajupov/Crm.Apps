using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Storages;
using Crm.Apps.Companies.V1.Requests;
using Crm.Apps.Companies.V1.Responses;
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

        public async Task<CompanyCommentGetPagedListResponse> GetPagedListAsync(
            CompanyCommentGetPagedListRequest request,
            CancellationToken ct)
        {
            var comments = _storage.CompanyComments
                .Where(x =>
                    x.CompanyId == request.CompanyId &&
                    (request.Value.IsEmpty() || EF.Functions.ILike(x.Value, $"{request.Value}%")) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new CompanyCommentGetPagedListResponse
            {
                TotalCount = await comments
                    .CountAsync(ct),
                Comments = await comments
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
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
