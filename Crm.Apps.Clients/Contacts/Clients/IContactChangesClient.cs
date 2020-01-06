using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Contacts.Models;
using Crm.Apps.Clients.Contacts.RequestParameters;

namespace Crm.Apps.Clients.Contacts.Clients
{
    public interface IContactChangesClient
    {
        Task<List<ContactChange>> GetPagedListAsync(
            ContactChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}