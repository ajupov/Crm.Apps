using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Companies.Models;
using Crm.Apps.Clients.Companies.RequestParameters;

namespace Crm.Apps.Clients.Companies.Clients
{
    public interface ICompanyCommentsClient
    {
        Task<List<CompanyComment>> GetPagedListAsync(
            CompanyCommentGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task CreateAsync(CompanyComment comment, CancellationToken ct = default);
    }
}