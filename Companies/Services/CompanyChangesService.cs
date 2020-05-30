using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Companies.Storages;
using Crm.Apps.Companies.V1.Requests;
using Crm.Apps.Companies.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Companies.Services
{
    public class CompanyChangesService : ICompanyChangesService
    {
        private readonly CompaniesStorage _storage;

        public CompanyChangesService(CompaniesStorage storage)
        {
            _storage = storage;
        }

        public async Task<CompanyChangeGetPagedListResponse> GetPagedListAsync(
            CompanyChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.CompanyChanges
                .Where(x =>
                    (request.CompanyId.IsEmpty() || x.CompanyId == request.CompanyId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new CompanyChangeGetPagedListResponse
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
