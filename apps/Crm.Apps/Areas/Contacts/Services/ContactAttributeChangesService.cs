using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Areas.Contacts.Models;
using Crm.Apps.Areas.Contacts.RequestParameters;
using Crm.Apps.Areas.Contacts.Storages;
using Crm.Apps.Utils;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Contacts.Services
{
    public class ContactAttributeChangesService : IContactAttributeChangesService
    {
        private readonly ContactsStorage _storage;

        public ContactAttributeChangesService(ContactsStorage storage)
        {
            _storage = storage;
        }

        public Task<List<ContactAttributeChange>> GetPagedListAsync(
            ContactAttributeChangeGetPagedListRequestParameter request, CancellationToken ct)
        {
            return _storage.ContactAttributeChanges
                .AsNoTracking()
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.AttributeId.IsEmpty() || x.AttributeId == request.AttributeId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }
    }
}