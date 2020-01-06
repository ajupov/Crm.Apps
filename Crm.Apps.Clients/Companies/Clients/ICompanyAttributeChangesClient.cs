using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Companies.Models;
using Crm.Apps.Clients.Companies.RequestParameters;

namespace Crm.Apps.Clients.Companies.Clients
{
    public interface ICompanyAttributeChangesClient
    {
        Task<List<CompanyAttributeChange>> GetPagedListAsync(
            CompanyAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}