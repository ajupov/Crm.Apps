using System;
using System.Threading.Tasks;

namespace Crm.Apps.Tests.Builders.Products
{
    public interface IProductStatusBuilder
    {
        ProductStatusBuilder WithAccountId(Guid accountId);

        ProductStatusBuilder WithName(string name);

        Task<Clients.Products.Models.ProductStatus> BuildAsync();
    }
}