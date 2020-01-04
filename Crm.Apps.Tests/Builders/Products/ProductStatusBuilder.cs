using System;
using System.Threading.Tasks;
using Crm.Clients.Products.Clients;
using Crm.Clients.Products.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Products
{
    public class ProductStatusBuilder : IProductStatusBuilder
    {
        private readonly IProductStatusesClient _productStatusesClient;
        private readonly ProductStatus _productStatus;

        public ProductStatusBuilder(IProductStatusesClient productStatusesClient)
        {
            _productStatusesClient = productStatusesClient;
            _productStatus = new ProductStatus
            {
                AccountId = Guid.Empty,
                Name = "Test"
            };
        }

        public ProductStatusBuilder WithAccountId(Guid accountId)
        {
            _productStatus.AccountId = accountId;

            return this;
        }

        public ProductStatusBuilder WithName(string name)
        {
            _productStatus.Name = name;

            return this;
        }

        public async Task<ProductStatus> BuildAsync()
        {
            if (_productStatus.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_productStatus.AccountId));
            }

            var createdId = await _productStatusesClient.CreateAsync(_productStatus);

            return await _productStatusesClient.GetAsync(createdId);
        }
    }
}