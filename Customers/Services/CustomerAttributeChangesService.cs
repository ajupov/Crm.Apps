using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Customers.Storages;
using Crm.Apps.Customers.V1.Requests;
using Crm.Apps.Customers.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Customers.Services
{
    public class CustomerAttributeChangesService : ICustomerAttributeChangesService
    {
        private readonly CustomersStorage _storage;

        public CustomerAttributeChangesService(CustomersStorage storage)
        {
            _storage = storage;
        }

        public async Task<CustomerAttributeChangeGetPagedListResponse> GetPagedListAsync(
            CustomerAttributeChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.CustomerAttributeChanges
                .AsNoTracking()
                .Where(x =>
                    (request.AttributeId.IsEmpty() || x.AttributeId == request.AttributeId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new CustomerAttributeChangeGetPagedListResponse
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
