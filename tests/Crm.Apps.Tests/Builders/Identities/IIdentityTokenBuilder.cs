using System;
using System.Threading.Tasks;

namespace Crm.Apps.Tests.Builders.Identities
{
    public interface IIdentityTokenBuilder
    {
        IdentityTokenBuilder WithIdentityId(Guid identityId);

        IdentityTokenBuilder WithType(string value);

        IdentityTokenBuilder WithExpirationDateTime(DateTime expirationDateTime);

        Task<Clients.Identities.Models.IdentityToken> BuildAsync();
    }
}