using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
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
            var queryable = _storage.ContactComments
                .AsNoTracking()
                .Where(x =>
                    x.ContactId == request.ContactId &&
                    (!request.BeforeCreateDateTime.HasValue || x.CreateDateTime < request.BeforeCreateDateTime) &&
                    (!request.AfterCreateDateTime.HasValue || x.CreateDateTime > request.AfterCreateDateTime));

            var minCreateDateTime = _storage.ContactComments
                .AsNoTracking()
                .Where(x => x.ContactId == request.ContactId)
                .Min(x => x != null ? x.CreateDateTime : (DateTime?) null);

            var comments = await queryable
                .SortBy(request.SortBy, request.OrderBy)
                .Take(request.Limit)
                .ToListAsync(ct);

            return new ContactCommentGetPagedListResponse
            {
                HasCommentsBefore = comments.Any() && minCreateDateTime < comments.Min(x => x.CreateDateTime),
                Comments = comments
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
