using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Contacts.Models;
using Crm.Apps.Clients.Contacts.RequestParameters;

namespace Crm.Apps.Clients.Contacts.Clients
{
    public interface IContactAttributeChangesClient
    {
        Task<List<ContactAttributeChange>> GetPagedListAsync(
            ContactAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}