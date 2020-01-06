using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Contacts.Models;
using Crm.Apps.Clients.Contacts.RequestParameters;

namespace Crm.Apps.Clients.Contacts.Clients
{
    public interface IContactCommentsClient
    {
        Task<List<ContactComment>> GetPagedListAsync(
            ContactCommentGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task CreateAsync(ContactComment comment, CancellationToken ct = default);
    }
}