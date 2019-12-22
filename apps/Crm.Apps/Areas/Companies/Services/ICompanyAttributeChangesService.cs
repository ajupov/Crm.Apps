using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.Parameters;

namespace Crm.Apps.Areas.Companies.Services
{
    public interface ICompanyAttributeChangesService
    {
        Task<List<CompanyAttributeChange>> GetPagedListAsync(
            CompanyAttributeChangeGetPagedListParameter parameter,
            CancellationToken ct);
    }
}