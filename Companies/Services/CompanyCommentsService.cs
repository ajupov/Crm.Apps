using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
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
            var queryable = _storage.CompanyComments
                .Where(x =>
                    x.CompanyId == request.CompanyId &&
                    (!request.BeforeCreateDateTime.HasValue || x.CreateDateTime < request.BeforeCreateDateTime) &&
                    (!request.AfterCreateDateTime.HasValue || x.CreateDateTime > request.AfterCreateDateTime));

            var minCreateDateTime = _storage.CompanyComments
                .Where(x => x.CompanyId == request.CompanyId)
                .Min(x => x.CreateDateTime);

            var comments = await queryable
                .SortBy(request.SortBy, request.OrderBy)
                .Take(request.Limit)
                .ToListAsync(ct);

            return new CompanyCommentGetPagedListResponse
            {
                HasCommentsBefore = minCreateDateTime < comments.Min(x => x.CreateDateTime),
                Comments = comments
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
