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
    public class CustomerChangesService : ICustomerChangesService
    {
        private readonly CustomersStorage _storage;

        public CustomerChangesService(CustomersStorage storage)
        {
            _storage = storage;
        }

        public async Task<CustomerChangeGetPagedListResponse> GetPagedListAsync(
            CustomerChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.CustomerChanges
                .AsNoTracking()
                .Where(x =>
                    (request.CustomerId.IsEmpty() || x.CustomerId == request.CustomerId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new CustomerChangeGetPagedListResponse
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
