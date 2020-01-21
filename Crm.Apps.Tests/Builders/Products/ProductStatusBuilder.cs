using System;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Products.Clients;
using Crm.Apps.v1.Clients.Products.Models;

namespace Crm.Apps.Tests.Builders.Products
{
    public class ProductStatusBuilder : IProductStatusBuilder
    {
        private readonly IProductStatusesClient _productStatusesClient;
        private readonly ProductStatus _status;

        public ProductStatusBuilder(IProductStatusesClient productStatusesClient)
        {
            _productStatusesClient = productStatusesClient;
            _status = new ProductStatus
            {
                AccountId = Guid.Empty,
                Name = "Test",
                IsDeleted = false
            };
        }

        public ProductStatusBuilder WithName(string name)
        {
            _status.Name = name;

            return this;
        }

        public ProductStatusBuilder IsDeleted()
        {
            _status.IsDeleted = true;

            return this;
        }

        public async Task<ProductStatus> BuildAsync()
        {
            var id = await _productStatusesClient.CreateAsync(_status);

            return await _productStatusesClient.GetAsync(id);
        }
    }
}