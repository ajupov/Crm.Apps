using System;
using System.Threading.Tasks;
using Crm.Clients.Products.Models;

namespace Crm.Apps.Tests.Builders.Products
{
    public interface IProductBuilder
    {
        ProductBuilder WithAccountId(Guid accountId);

        ProductBuilder WithParentProductId(Guid productId);

        ProductBuilder WithType(ProductType type);

        ProductBuilder WithStatusId(Guid statusId);

        ProductBuilder WithName(string name);

        ProductBuilder WithVendorCode(string vendorCode);

        ProductBuilder WithPrice(decimal price);

        ProductBuilder AsHidden();

        ProductBuilder AsDeleted();

        ProductBuilder WithAttributeLink(Guid attributeId, string value);

        ProductBuilder WithCategoryLink(Guid categoryId);

        Task<Product> BuildAsync();
    }
}