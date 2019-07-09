using System;
using System.Threading.Tasks;
using Crm.Clients.Products.Models;
using Crm.Common.Types;

namespace Crm.Apps.Tests.Builders.Products
{
    public interface IProductAttributeBuilder
    {
        ProductAttributeBuilder WithAccountId(Guid accountId);

        ProductAttributeBuilder WithType(AttributeType type);
        
        ProductAttributeBuilder WithKey(string key);

        Task<ProductAttribute> BuildAsync();
    }
}