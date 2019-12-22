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
    public class ContactChangesService : IContactChangesService
    {
        private readonly ContactsStorage _storage;

        public ContactChangesService(ContactsStorage storage)
        {
            _storage = storage;
        }

        public Task<List<ContactChange>> GetPagedListAsync(ContactChangeGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _storage.ContactChanges
                .AsNoTracking()
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.ContactId.IsEmpty() || x.ContactId == request.ContactId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }
    }
}