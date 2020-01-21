using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.v1.Models;
using Crm.Apps.Companies.v1.RequestParameters;

namespace Crm.Apps.Companies.Services
{
    public interface ICompanyAttributeChangesService
    {
        Task<List<CompanyAttributeChange>> GetPagedListAsync(
            CompanyAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}