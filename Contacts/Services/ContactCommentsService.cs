using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.Storages;
using Crm.Apps.Contacts.V1.Requests;
using Crm.Apps.Contacts.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Contacts.Services
{
    public class ContactCommentsService : IContactCommentsService
    {
        private readonly ContactsStorage _storage;

        public ContactCommentsService(ContactsStorage storage)
        {
            _storage = storage;
        }

        public async Task<ContactCommentGetPagedListResponse> GetPagedListAsync(
            ContactCommentGetPagedListRequest request,
            CancellationToken ct)
        {
            var comments = _storage.ContactComments
                .Where(x =>
                    x.ContactId == request.ContactId &&
                    (request.Value.IsEmpty() || EF.Functions.ILike(x.Value, $"{request.Value}%")) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new ContactCommentGetPagedListResponse
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

        public async Task CreateAsync(Guid userId, ContactComment comment, CancellationToken ct)
        {
            var newComment = new ContactComment
            {
                Id = Guid.NewGuid(),
                ContactId = comment.ContactId,
                CommentatorUserId = userId,
                Value = comment.Value,
                CreateDateTime = DateTime.UtcNow
            };

            await _storage.AddAsync(newComment, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
