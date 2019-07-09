using System;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Common.Types;

namespace Crm.Apps.Tests.Builders.Users
{
    public interface IUserAttributeBuilder
    {
        UserAttributeBuilder WithAccountId(Guid accountId);

        UserAttributeBuilder WithType(AttributeType type);
        
        UserAttributeBuilder WithKey(string key);

        Task<UserAttribute> BuildAsync();
    }
}