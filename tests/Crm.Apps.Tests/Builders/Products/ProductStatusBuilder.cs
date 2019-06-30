using System;
using System.Threading.Tasks;
using Crm.Clients.Products.Clients;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Products
{
    public class ProductStatusBuilder : IProductStatusBuilder
    {
        private readonly Clients.Products.Models.ProductStatus _productStatus;
        private readonly IProductStatusesClient _productStatusesClient;

        public ProductStatusBuilder(IProductStatusesClient productStatusesClient)
        {
            _productStatusesClient = productStatusesClient;
            _productStatus = new Clients.Products.Models.ProductStatus
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

        public async Task<Clients.Products.Models.ProductStatus> BuildAsync()
        {
            if (_productStatus.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_productStatus.AccountId));
            }

            var createdId = await _productStatusesClient.CreateAsync(_productStatus).ConfigureAwait(false);

            return await _productStatusesClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}