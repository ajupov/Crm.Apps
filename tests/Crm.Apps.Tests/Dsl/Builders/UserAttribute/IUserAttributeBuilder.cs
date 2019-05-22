using System;
using System.Threading.Tasks;

namespace Crm.Apps.Tests.Dsl.Builders.UserAttribute
{
    public interface IUserAttributeBuilder
    {
        UserAttributeBuilder WithAccountId(Guid accountId);

        UserAttributeBuilder WithKey(string key);

        Task<Clients.Users.Models.UserAttribute> BuildAsync();
    }
}