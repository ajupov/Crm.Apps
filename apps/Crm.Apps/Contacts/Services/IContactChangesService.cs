using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.Parameters;

namespace Crm.Apps.Contacts.Services
{
    public interface IContactChangesService
    {
        Task<List<ContactChange>> GetPagedListAsync(ContactChangeGetPagedListParameter parameter, CancellationToken ct);
    }
}