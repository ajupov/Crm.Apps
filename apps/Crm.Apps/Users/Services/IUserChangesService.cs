using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.Parameters;

namespace Crm.Apps.Users.Services
{
    public interface IUserChangesService
    {
        Task<List<UserChange>> GetPagedListAsync(UserChangeGetPagedListParameter parameter, CancellationToken ct);
    }
}