using System;
using System.Threading.Tasks;

namespace Crm.Apps.Tests.Builders.Products
{
    public interface IProductCategoryBuilder
    {
        ProductCategoryBuilder WithAccountId(Guid accountId);

        ProductCategoryBuilder WithName(string name);

        Task<Clients.Products.Models.ProductCategory> BuildAsync();
    }
}