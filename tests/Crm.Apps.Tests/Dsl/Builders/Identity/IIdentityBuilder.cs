using System;
using System.Threading.Tasks;
using Crm.Clients.Identities.Models;

namespace Crm.Apps.Tests.Dsl.Builders.Identity
{
    public interface IIdentityBuilder
    {
        IdentityBuilder WithUserId(Guid userId);

        IdentityBuilder WithType(IdentityType type);

        IdentityBuilder WithKey(string key);

        IdentityBuilder WithPassword(string value);
        
        IdentityBuilder AsPrimary();

        IdentityBuilder AsVerified();

        Task<Clients.Identities.Models.Identity> BuildAsync();
    }
}