using System.Threading.Tasks;
using Crm.Apps.Clients.Products.Models;

namespace Crm.Apps.Tests.Builders.Products
{
    public interface IProductStatusBuilder
    {
        ProductStatusBuilder WithName(string name);

        ProductStatusBuilder IsDeleted();

        Task<ProductStatus> BuildAsync();
    }
}