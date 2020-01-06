using System.Threading.Tasks;
using Crm.Apps.Clients.Users.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Users
{
    public interface IUserAttributeBuilder
    {
        UserAttributeBuilder WithType(AttributeType type);

        UserAttributeBuilder WithKey(string key);

        UserAttributeBuilder AsDeleted();

        Task<UserAttribute> BuildAsync();
    }
}