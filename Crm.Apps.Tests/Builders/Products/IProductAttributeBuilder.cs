using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Products.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Products
{
    public interface IProductAttributeBuilder
    {
        ProductAttributeBuilder WithType(AttributeType type);

        ProductAttributeBuilder WithKey(string key);

        ProductAttributeBuilder AsDeleted();

        Task<ProductAttribute> BuildAsync();
    }
}