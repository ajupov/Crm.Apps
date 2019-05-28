using System;
using System.Threading.Tasks;
using Crm.Common.Types;

namespace Crm.Apps.Tests.Dsl.Builders.UserAttribute
{
    public interface IUserAttributeBuilder
    {
        UserAttributeBuilder WithAccountId(Guid accountId);

        UserAttributeBuilder WithType(AttributeType type);
        
        UserAttributeBuilder WithKey(string key);

        Task<Clients.Users.Models.UserAttribute> BuildAsync();
    }
}