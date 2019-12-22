using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Contacts.Models;
using Crm.Apps.Areas.Contacts.Parameters;

namespace Crm.Apps.Areas.Contacts.Services
{
    public interface IContactChangesService
    {
        Task<List<ContactChange>> GetPagedListAsync(ContactChangeGetPagedListParameter parameter, CancellationToken ct);
    }
}