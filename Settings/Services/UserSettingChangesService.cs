using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Settings.V1.Requests;
using Crm.Apps.Settings.V1.Responses;

namespace Crm.Apps.Settings.Services
{
    public class UserSettingChangesService : IUserSettingChangesService
    {
        public Task<UserSettingChangeGetPagedListResponse> GetPagedListAsync(
            Guid userId,
            UserSettingChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
