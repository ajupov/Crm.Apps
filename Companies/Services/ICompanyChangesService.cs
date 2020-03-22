using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.v1.Requests;
using Crm.Apps.Companies.v1.Responses;

namespace Crm.Apps.Companies.Services
{
    public interface ICompanyChangesService
    {
        Task<CompanyChangeGetPagedListResponse> GetPagedListAsync(
            CompanyChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}