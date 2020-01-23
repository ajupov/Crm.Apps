using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Contacts.Models;
using Crm.Apps.v1.Clients.Contacts.RequestParameters;

namespace Crm.Apps.v1.Clients.Contacts.Clients
{
    public interface IContactAttributeChangesClient
    {
        Task<List<ContactAttributeChange>> GetPagedListAsync(
            string accessToken,
            ContactAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}