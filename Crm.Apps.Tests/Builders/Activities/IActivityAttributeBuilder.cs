using System;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;
using Crm.Common.Types;

namespace Crm.Apps.Tests.Builders.Activities
{
    public interface IActivityAttributeBuilder
    {
        ActivityAttributeBuilder WithAccountId(Guid accountId);

        ActivityAttributeBuilder WithType(AttributeType type);

        ActivityAttributeBuilder WithKey(string key);

        Task<ActivityAttribute> BuildAsync();
    }
}