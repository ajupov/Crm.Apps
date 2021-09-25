using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Customers.Models;
using Crm.Apps.Customers.Storages;
using Crm.Apps.Customers.V1.Requests;
using Crm.Apps.Customers.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Customers.Services
{
    public class CustomerCommentsService : ICustomerCommentsService
    {
        private readonly CustomersStorage _storage;

        public CustomerCommentsService(CustomersStorage storage)
        {
            _storage = storage;
        }

        public async Task<CustomerCommentGetPagedListResponse> GetPagedListAsync(
            CustomerCommentGetPagedListRequest request,
            CancellationToken ct)
        {
            var queryable = _storage.CustomerComments
                .AsNoTracking()
                .Where(x =>
                    x.CustomerId == request.CustomerId &&
                    (!request.BeforeCreateDateTime.HasValue || x.CreateDateTime < request.BeforeCreateDateTime) &&
                    (!request.AfterCreateDateTime.HasValue || x.CreateDateTime > request.AfterCreateDateTime));

            var minCreateDateTime = _storage.CustomerComments
                .AsNoTracking()
                .Where(x => x.CustomerId == request.CustomerId)
                .Min(x => x != null ? x.CreateDateTime : (DateTime?) null);

            var comments = await queryable
                .SortBy(request.SortBy, request.OrderBy)
                .Take(request.Limit)
                .ToListAsync(ct);

            return new CustomerCommentGetPagedListResponse
            {
                HasCommentsBefore = comments.Any() && minCreateDateTime < comments.Min(x => x.CreateDateTime),
                Comments = comments
            };
        }

        public async Task CreateAsync(Guid userId, CustomerComment comment, CancellationToken ct)
        {
            var newComment = new CustomerComment
            {
                Id = Guid.NewGuid(),
                CustomerId = comment.CustomerId,
                CommentatorUserId = userId,
                Value = comment.Value,
                CreateDateTime = DateTime.UtcNow
            };

            await _storage.AddAsync(newComment, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
