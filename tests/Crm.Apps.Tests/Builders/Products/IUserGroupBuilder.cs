using System;
using System.Threading.Tasks;
using Crm.Clients.Products.Models;

namespace Crm.Apps.Tests.Builders.Products
{
    public interface IProductCategoryBuilder
    {
        ProductCategoryBuilder WithAccountId(Guid accountId);

        ProductCategoryBuilder WithName(string name);

        Task<ProductCategory> BuildAsync();
    }
}