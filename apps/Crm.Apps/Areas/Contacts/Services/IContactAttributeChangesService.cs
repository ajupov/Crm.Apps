using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Contacts.Models;
using Crm.Apps.Areas.Contacts.RequestParameters;

namespace Crm.Apps.Areas.Contacts.Services
{
    public interface IContactAttributeChangesService
    {
        Task<List<ContactAttributeChange>> GetPagedListAsync(
            ContactAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}