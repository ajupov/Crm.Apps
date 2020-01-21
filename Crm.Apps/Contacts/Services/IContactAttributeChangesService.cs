using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Contacts.v1.Models;
using Crm.Apps.Contacts.v1.RequestParameters;

namespace Crm.Apps.Contacts.Services
{
    public interface IContactAttributeChangesService
    {
        Task<List<ContactAttributeChange>> GetPagedListAsync(
            ContactAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}