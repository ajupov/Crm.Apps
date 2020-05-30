using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.V1.Requests;
using Crm.Apps.Contacts.V1.Responses;

namespace Crm.Apps.Contacts.Services
{
    public interface IContactCommentsService
    {
        Task<ContactCommentGetPagedListResponse> GetPagedListAsync(
            ContactCommentGetPagedListRequest request,
            CancellationToken ct);

        Task CreateAsync(Guid userId, ContactComment comment, CancellationToken ct);
    }
}
