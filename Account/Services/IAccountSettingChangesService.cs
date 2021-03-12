using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Account.V1.Requests;
using Crm.Apps.Account.V1.Responses;

namespace Crm.Apps.Account.Services
{
    public interface IAccountSettingChangesService
    {
        Task<AccountSettingChangeGetPagedListResponse> GetPagedListAsync(Guid accountId,
            AccountSettingChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}
