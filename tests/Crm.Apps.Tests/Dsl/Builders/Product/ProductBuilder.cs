using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crm.Clients.Products.Clients;
using Crm.Clients.Products.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Dsl.Builders.Product
{
    public class ProductBuilder : IProductBuilder
    {
        private readonly Clients.Products.Models.Product _product;
        private readonly IProductsClient _productsClient;

        public ProductBuilder(IProductsClient productsClient)
        {
            _productsClient = productsClient;
            _product = new Clients.Products.Models.Product
            {
                AccountId = Guid.Empty,
                ParentProductId = Guid.Empty,
                Type = ProductType.Material,
                Name = "Test",
                VendorCode = "Test",
                Price = 1,
                IsHidden = false,
                IsDeleted = false
            };
        }

        public ProductBuilder WithAccountId(Guid accountId)
        {
            _product.AccountId = accountId;

            return this;
        }

        public ProductBuilder WithParentProductId(Guid productId)
        {
            _product.ParentProductId = productId;

            return this;
        }

        public ProductBuilder WithType(ProductType type)
        {
            _product.Type = type;

            return this;
        }

        public ProductBuilder WithStatusId(Guid statusId)
        {
            _product.StatusId = statusId;

            return this;
        }

        public ProductBuilder WithName(string name)
        {
            _product.Name = name;

            return this;
        }

        public ProductBuilder WithVendorCode(string vendorCode)
        {
            _product.VendorCode = vendorCode;

            return this;
        }

        public ProductBuilder WithPrice(decimal price)
        {
            _product.Price = price;

            return this;
        }

        public ProductBuilder AsHidden()
        {
            _product.IsHidden = true;

            return this;
        }

        public ProductBuilder AsDeleted()
        {
            _product.IsDeleted = true;

            return this;
        }

        public ProductBuilder WithAttributeLink(Guid attributeId, string value)
        {
            if (_product.AttributeLinks == null)
            {
                _product.AttributeLinks = new List<ProductAttributeLink>();
            }

            _product.AttributeLinks.Add(new ProductAttributeLink
            {
                ProductAttributeId = attributeId,
                Value = value
            });

            return this;
        }

        public ProductBuilder WithCategoryLink(Guid categoryId)
        {
            if (_product.CategoryLinks == null)
            {
                _product.CategoryLinks = new List<ProductCategoryLink>();
            }

            _product.CategoryLinks.Add(new ProductCategoryLink
            {
                ProductCategoryId = categoryId
            });

            return this;
        }

        public async Task<Clients.Products.Models.Product> BuildAsync()
        {
            if (_product.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_product.AccountId));
            }

            var createdId = await _productsClient.CreateAsync(_product).ConfigureAwait(false);

            return await _productsClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}