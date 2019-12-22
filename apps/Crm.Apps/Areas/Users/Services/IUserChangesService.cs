using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.RequestParameters;

namespace Crm.Apps.Areas.Users.Services
{
    public interface IUserChangesService
    {
        Task<List<UserChange>> GetPagedListAsync(UserChangeGetPagedListRequestParameter request, CancellationToken ct);
    }
}