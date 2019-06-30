using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Contacts.Helpers;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.Parameters;
using Crm.Apps.Contacts.Storages;
using Crm.Utils.Guid;
using Crm.Utils.String;
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

        public Task<List<ContactComment>> GetPagedListAsync(ContactCommentGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.ContactComments.Where(x =>
                    x.ContactId == parameter.ContactId &&
                    (parameter.CommentatorUserId.IsEmpty() || x.CommentatorUserId == parameter.CommentatorUserId) &&
                    (parameter.Value.IsEmpty() || EF.Functions.Like(x.Value, $"{parameter.Value}%")) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
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

            await _storage.AddAsync(newComment, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}