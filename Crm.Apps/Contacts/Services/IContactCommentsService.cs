using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.RequestParameters;

namespace Crm.Apps.Contacts.Services
{
    public interface IContactCommentsService
    {
        Task<List<ContactComment>> GetPagedListAsync(
            ContactCommentGetPagedListRequestParameter request,
            CancellationToken ct);

        Task CreateAsync(Guid userId, ContactComment comment, CancellationToken ct);
    }
}