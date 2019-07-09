using System;
using System.Threading.Tasks;
using Crm.Clients.Identities.Models;

namespace Crm.Apps.Tests.Builders.Identities
{
    public interface IIdentityTokenBuilder
    {
        IdentityTokenBuilder WithIdentityId(Guid identityId);

        IdentityTokenBuilder WithType(string value);

        IdentityTokenBuilder WithExpirationDateTime(DateTime expirationDateTime);

        Task<IdentityToken> BuildAsync();
    }
}