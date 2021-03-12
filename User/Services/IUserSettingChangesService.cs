using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.User.V1.Requests;
using Crm.Apps.User.V1.Responses;

namespace Crm.Apps.User.Services
{
    public interface IUserSettingChangesService
    {
        Task<UserSettingChangeGetPagedListResponse> GetPagedListAsync(
            Guid userId,
            UserSettingChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}
