using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Contacts.Models;
using Crm.Apps.v1.Clients.Contacts.RequestParameters;

namespace Crm.Apps.v1.Clients.Contacts.Clients
{
    public interface IContactCommentsClient
    {
        Task<List<ContactComment>> GetPagedListAsync(
            ContactCommentGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task CreateAsync(ContactComment comment, CancellationToken ct = default);
    }
}