using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Common.Types;

namespace Crm.Clients.Users.Clients.UserAttributes
{
    public interface IUserAttributesClient
    {
        Task<List<AttributeType>> GetTypesAsync(CancellationToken ct = default);

        Task<UserAttribute> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<UserAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<UserAttribute>> GetPagedListAsync(Guid? accountId = default, List<AttributeType> types = default,
            string key = default, bool? isDeleted = default, DateTime? minCreateDate = default, 
            DateTime? maxCreateDate = default, int offset = default, int limit = 10, string sortBy = default, 
            string orderBy = default, CancellationToken ct = default);

        Task<Guid> CreateAsync(UserAttribute attribute, CancellationToken ct = default);

        Task UpdateAsync(UserAttribute attribute, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}