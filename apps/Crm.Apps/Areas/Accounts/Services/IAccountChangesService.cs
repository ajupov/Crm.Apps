using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Apps.Areas.Accounts.RequestParameters;

namespace Crm.Apps.Areas.Accounts.Services
{
    public interface IAccountChangesService
    {
        Task<List<AccountChange>> GetPagedListAsync(AccountChangeGetPagedListRequestParameter request, CancellationToken ct);
    }
}