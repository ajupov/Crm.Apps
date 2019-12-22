using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Parameters;

namespace Crm.Apps.Areas.Users.Services
{
    public interface IUserAttributeChangesService
    {
        Task<List<UserAttributeChange>> GetPagedListAsync(UserAttributeChangeGetPagedListParameter parameter,
            CancellationToken ct);
    }
}