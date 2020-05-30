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
    public class ContactAttributeChangesService : IContactAttributeChangesService
    {
        private readonly ContactsStorage _storage;

        public ContactAttributeChangesService(ContactsStorage storage)
        {
            _storage = storage;
        }

        public async Task<ContactAttributeChangeGetPagedListResponse> GetPagedListAsync(
            ContactAttributeChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.ContactAttributeChanges
                .Where(x =>
                    (request.AttributeId.IsEmpty() || x.AttributeId == request.AttributeId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new ContactAttributeChangeGetPagedListResponse
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
