using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.RequestParameters;

namespace Crm.Apps.Users.Services
{
    public interface IUserGroupChangesService
    {
        Task<List<UserGroupChange>> GetPagedListAsync(UserGroupChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}