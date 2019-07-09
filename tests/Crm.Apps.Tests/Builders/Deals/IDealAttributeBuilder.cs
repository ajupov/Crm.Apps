using System;
using System.Threading.Tasks;
using Crm.Clients.Deals.Models;
using Crm.Common.Types;

namespace Crm.Apps.Tests.Builders.Deals
{
    public interface IDealAttributeBuilder
    {
        DealAttributeBuilder WithAccountId(Guid accountId);

        DealAttributeBuilder WithType(AttributeType type);

        DealAttributeBuilder WithKey(string key);

        Task<DealAttribute> BuildAsync();
    }
}