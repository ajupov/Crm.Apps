using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Companies.Storages;
using Crm.Apps.Companies.v1.Requests;
using Crm.Apps.Companies.v1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Companies.Services
{
    public class CompanyAttributeChangesService : ICompanyAttributeChangesService
    {
        private readonly CompaniesStorage _storage;

        public CompanyAttributeChangesService(CompaniesStorage storage)
        {
            _storage = storage;
        }

        public async Task<CompanyAttributeChangeGetPagedListResponse> GetPagedListAsync(
            CompanyAttributeChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.CompanyAttributeChanges
                .Where(x =>
                    (request.AttributeId.IsEmpty() || x.AttributeId == request.AttributeId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new CompanyAttributeChangeGetPagedListResponse
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