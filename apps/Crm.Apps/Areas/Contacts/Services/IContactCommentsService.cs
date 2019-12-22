using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Contacts.Models;
using Crm.Apps.Areas.Contacts.Parameters;

namespace Crm.Apps.Areas.Contacts.Services
{
    public interface IContactCommentsService
    {
        Task<List<ContactComment>> GetPagedListAsync(
            ContactCommentGetPagedListParameter parameter,
            CancellationToken ct);

        Task CreateAsync(Guid userId, ContactComment comment, CancellationToken ct);
    }
}