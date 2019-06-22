using System;
using System.Threading.Tasks;
using Crm.Common.Types;

namespace Crm.Apps.Tests.Dsl.Builders.ProductAttribute
{
    public interface IProductAttributeBuilder
    {
        ProductAttributeBuilder WithAccountId(Guid accountId);

        ProductAttributeBuilder WithType(AttributeType type);
        
        ProductAttributeBuilder WithKey(string key);

        Task<Clients.Products.Models.ProductAttribute> BuildAsync();
    }
}