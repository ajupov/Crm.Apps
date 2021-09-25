using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Orders.Models;
using Crm.Apps.Orders.Storages;
using Crm.Apps.Orders.V1.Requests;
using Crm.Apps.Orders.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Orders.Services
{
    public class OrderCommentsService : IOrderCommentsService
    {
        private readonly OrdersStorage _storage;

        public OrderCommentsService(OrdersStorage storage)
        {
            _storage = storage;
        }

        public async Task<OrderCommentGetPagedListResponse> GetPagedListAsync(
            OrderCommentGetPagedListRequest request,
            CancellationToken ct)
        {
            var queryable = _storage.OrderComments
                .AsNoTracking()
                .Where(x =>
                    x.OrderId == request.OrderId &&
                    (!request.BeforeCreateDateTime.HasValue || x.CreateDateTime < request.BeforeCreateDateTime) &&
                    (!request.AfterCreateDateTime.HasValue || x.CreateDateTime > request.AfterCreateDateTime));

            var minCreateDateTime = _storage.OrderComments
                .AsNoTracking()
                .Where(x => x.OrderId == request.OrderId)
                .Min(x => x != null ? x.CreateDateTime : (DateTime?) null);

            var comments = await queryable
                .SortBy(request.SortBy, request.OrderBy)
                .Take(request.Limit)
                .ToListAsync(ct);

            return new OrderCommentGetPagedListResponse
            {
                HasCommentsBefore = comments.Any() && minCreateDateTime < comments.Min(x => x.CreateDateTime),
                Comments = comments
            };
        }

        public async Task CreateAsync(Guid userId, OrderComment comment, CancellationToken ct)
        {
            var newComment = new OrderComment
            {
                Id = Guid.NewGuid(),
                OrderId = comment.OrderId,
                CommentatorUserId = userId,
                Value = comment.Value,
                CreateDateTime = DateTime.UtcNow
            };

            await _storage.AddAsync(newComment, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
