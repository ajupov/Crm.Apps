using System;
using System.Threading.Tasks;

namespace Crm.Apps.Tests.Dsl.Builders.ProductStatus
{
    public interface IProductStatusBuilder
    {
        ProductStatusBuilder WithAccountId(Guid accountId);

        ProductStatusBuilder WithName(string name);

        Task<Clients.Products.Models.ProductStatus> BuildAsync();
    }
}