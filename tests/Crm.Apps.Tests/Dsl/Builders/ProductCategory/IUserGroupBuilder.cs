using System;
using System.Threading.Tasks;

namespace Crm.Apps.Tests.Dsl.Builders.ProductCategory
{
    public interface IProductCategoryBuilder
    {
        ProductCategoryBuilder WithAccountId(Guid accountId);

        ProductCategoryBuilder WithName(string name);

        Task<Clients.Products.Models.ProductCategory> BuildAsync();
    }
}