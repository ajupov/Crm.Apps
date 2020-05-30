using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.V1.Requests;
using Crm.Apps.Companies.V1.Responses;

namespace Crm.Apps.Companies.Services
{
    public interface ICompanyAttributeChangesService
    {
        Task<CompanyAttributeChangeGetPagedListResponse> GetPagedListAsync(
            CompanyAttributeChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}
