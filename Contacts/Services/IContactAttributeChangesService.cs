using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Contacts.V1.Requests;
using Crm.Apps.Contacts.V1.Responses;

namespace Crm.Apps.Contacts.Services
{
    public interface IContactAttributeChangesService
    {
        Task<ContactAttributeChangeGetPagedListResponse> GetPagedListAsync(
            ContactAttributeChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}
