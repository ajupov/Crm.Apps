using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Apps.Areas.Accounts.Parameters;

namespace Crm.Apps.Areas.Accounts.Services
{
    public interface IAccountChangesService
    {
        Task<List<AccountChange>> GetPagedListAsync(AccountChangeGetPagedListParameter parameter, CancellationToken ct);
    }
}