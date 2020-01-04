using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.RequestParameters;

namespace Crm.Apps.Contacts.Services
{
    public interface IContactAttributeChangesService
    {
        Task<List<ContactAttributeChange>> GetPagedListAsync(
            ContactAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}