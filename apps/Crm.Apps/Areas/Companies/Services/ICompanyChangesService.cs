using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.Parameters;

namespace Crm.Apps.Areas.Companies.Services
{
    public interface ICompanyChangesService
    {
        Task<List<CompanyChange>> GetPagedListAsync(CompanyChangeGetPagedListParameter parameter, CancellationToken ct);
    }
}