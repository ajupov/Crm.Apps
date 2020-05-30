using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Contacts.Storages;
using Crm.Apps.Contacts.V1.Requests;
using Crm.Apps.Contacts.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Contacts.Services
{
    public class ContactChangesService : IContactChangesService
    {
        private readonly ContactsStorage _storage;

        public ContactChangesService(ContactsStorage storage)
        {
            _storage = storage;
        }

        public async Task<ContactChangeGetPagedListResponse> GetPagedListAsync(
            ContactChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.ContactChanges
                .Where(x =>
                    (request.ContactId.IsEmpty() || x.ContactId == request.ContactId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new ContactChangeGetPagedListResponse
            {
                TotalCount = await changes
                    .CountAsync(ct),
                Changes = await changes
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }
    }
}
