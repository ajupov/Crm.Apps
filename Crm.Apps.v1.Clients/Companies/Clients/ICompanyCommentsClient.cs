using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Companies.Models;
using Crm.Apps.v1.Clients.Companies.RequestParameters;

namespace Crm.Apps.v1.Clients.Companies.Clients
{
    public interface ICompanyCommentsClient
    {
        Task<List<CompanyComment>> GetPagedListAsync(
            CompanyCommentGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task CreateAsync(CompanyComment comment, CancellationToken ct = default);
    }
}