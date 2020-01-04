using System;
using System.Threading.Tasks;
using Crm.Clients.Products.Models;

namespace Crm.Apps.Tests.Builders.Products
{
    public interface IProductStatusBuilder
    {
        ProductStatusBuilder WithAccountId(Guid accountId);

        ProductStatusBuilder WithName(string name);

        Task<ProductStatus> BuildAsync();
    }
}