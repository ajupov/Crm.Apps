using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Crm.Apps.Clients.Products.Clients;
using Crm.Apps.Clients.Products.Models;
using Crm.Apps.Tests.Creator;
using Xunit;

namespace Crm.Apps.Tests.Tests.Products
{
    public class ProductsTests
    {
        private readonly ICreate _create;
        private readonly IProductsClient _productsClient;

        public ProductsTests(ICreate create, IProductsClient productsClient)
        {
            _create = create;
            _productsClient = productsClient;
        }

        [Fact]
        public async Task WhenGetTypes_ThenSuccess()
        {
            var types = await _productsClient.GetTypesAsync();

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            
            var status = await _create.ProductStatus.BuildAsync();
            var productId = (await _create.Product.WithStatusId(status.Id).BuildAsync()
                ).Id;

            var product = await _productsClient.GetAsync(productId);

            Assert.NotNull(product);
            Assert.Equal(productId, product.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            
            var status = await _create.ProductStatus.BuildAsync();
            var productIds = (await Task.WhenAll(
                    _create.Product.WithStatusId(status.Id).WithName("Test1").BuildAsync(),
                    _create.Product.WithStatusId(status.Id).WithName("Test2").BuildAsync()))
                .Select(x => x.Id).ToList();

            var products = await _productsClient.GetListAsync(productIds);

            Assert.NotEmpty(products);
            Assert.Equal(productIds.Count, products.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            
            var attribute = await _create.ProductAttribute.BuildAsync();
            var category = await _create.ProductCategory.BuildAsync();
            var status = await _create.ProductStatus.BuildAsync();
            await Task.WhenAll(
                _create.Product.WithStatusId(status.Id).WithName("Test1")
                    .WithAttributeLink(attribute.Id, "Test").WithCategoryLink(category.Id).BuildAsync(),
                _create.Product.WithStatusId(status.Id).WithName("Test2")
                    .WithAttributeLink(attribute.Id, "Test").WithCategoryLink(category.Id).BuildAsync());

            var filterAttributes = new Dictionary<Guid, string> {{attribute.Id, "Test"}};
            var filterCategoryIds = new List<Guid> {category.Id};

            var products = await _productsClient.GetPagedListAsync(account.Id, sortBy: "CreateDateTime",
                orderBy: "desc", allAttributes: false, attributes: filterAttributes, allCategoryIds: false,
                categoryIds: filterCategoryIds);

            var results = products.Skip(1)
                .Zip(products, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(products);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            
            var attribute = await _create.ProductAttribute.BuildAsync();
            var category = await _create.ProductCategory.BuildAsync();
            var status = await _create.ProductStatus.BuildAsync();

            var product = new Product
            {
                AccountId = account.Id,
                ParentProductId = Guid.Empty,
                Type = ProductType.None,
                StatusId = status.Id,
                Name = "Test",
                VendorCode = "Test",
                Price = 1,
                Image = null,
                IsHidden = true,
                IsDeleted = true,
                AttributeLinks = new List<ProductAttributeLink>
                {
                    new ProductAttributeLink
                    {
                        ProductAttributeId = attribute.Id,
                        Value = "Test"
                    }
                },
                CategoryLinks = new List<ProductCategoryLink>
                {
                    new ProductCategoryLink
                    {
                        ProductCategoryId = category.Id
                    }
                }
            };

            var createdProductId = await _productsClient.CreateAsync(product);

            var createdProduct = await _productsClient.GetAsync(createdProductId);

            Assert.NotNull(createdProduct);
            Assert.Equal(createdProductId, createdProduct.Id);
            Assert.Equal(product.AccountId, createdProduct.AccountId);
            Assert.Equal(product.ParentProductId, Guid.Empty);
            Assert.Equal(product.Type, createdProduct.Type);
            Assert.Equal(product.StatusId, createdProduct.StatusId);
            Assert.Equal(product.Name, createdProduct.Name);
            Assert.Equal(product.VendorCode, createdProduct.VendorCode);
            Assert.Equal(product.Price, createdProduct.Price);
            Assert.Equal(product.IsHidden, createdProduct.IsHidden);
            Assert.Equal(product.IsDeleted, createdProduct.IsDeleted);
            Assert.True(createdProduct.CreateDateTime.IsMoreThanMinValue());
            Assert.NotEmpty(createdProduct.AttributeLinks);
            Assert.NotEmpty(createdProduct.CategoryLinks);
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            
            var status = await _create.ProductStatus.BuildAsync();
            var product = await _create.Product.WithStatusId(status.Id).BuildAsync()
                ;
            var attribute = await _create.ProductAttribute.BuildAsync();
            var category = await _create.ProductCategory.BuildAsync();

            product.StatusId = status.Id;
            product.Type = ProductType.Material;
            product.Name = "Test2";
            product.VendorCode = "Test2";
            product.Price = 2;
            product.IsHidden = true;
            product.IsDeleted = true;
            product.AttributeLinks.Add(new ProductAttributeLink {ProductAttributeId = attribute.Id, Value = "Test"});
            product.CategoryLinks.Add(new ProductCategoryLink {ProductCategoryId = category.Id});
            await _productsClient.UpdateAsync(product);

            var updatedProduct = await _productsClient.GetAsync(product.Id);

            Assert.Equal(product.StatusId, updatedProduct.StatusId);
            Assert.Equal(product.Type, updatedProduct.Type);
            Assert.Equal(product.Name, updatedProduct.Name);
            Assert.Equal(product.VendorCode, updatedProduct.VendorCode);
            Assert.Equal(product.Price, updatedProduct.Price);
            Assert.Equal(product.IsHidden, updatedProduct.IsHidden);
            Assert.Equal(product.IsDeleted, updatedProduct.IsDeleted);
            Assert.Equal(product.AttributeLinks.Single().ProductAttributeId,
                updatedProduct.AttributeLinks.Single().ProductAttributeId);
            Assert.Equal(product.AttributeLinks.Single().Value, updatedProduct.AttributeLinks.Single().Value);
            Assert.Equal(product.CategoryLinks.Single().ProductCategoryId,
                updatedProduct.CategoryLinks.Single().ProductCategoryId);
        }

        [Fact]
        public async Task WhenHide_ThenSuccess()
        {
            
            var status = await _create.ProductStatus.BuildAsync();
            var productIds = (await Task.WhenAll(
                    _create.Product.WithStatusId(status.Id).WithName("Test1").BuildAsync(),
                    _create.Product.WithStatusId(status.Id).WithName("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            await _productsClient.HideAsync(productIds);

            var products = await _productsClient.GetListAsync(productIds);

            Assert.All(products, x => Assert.True(x.IsHidden));
        }

        [Fact]
        public async Task WhenShow_ThenSuccess()
        {
            
            var status = await _create.ProductStatus.BuildAsync();

            var productIds = (await Task.WhenAll(
                    _create.Product.WithStatusId(status.Id).WithName("Test1").BuildAsync(),
                    _create.Product.WithStatusId(status.Id).WithName("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            await _productsClient.ShowAsync(productIds);

            var products = await _productsClient.GetListAsync(productIds);

            Assert.All(products, x => Assert.False(x.IsHidden));
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            
            var status = await _create.ProductStatus.BuildAsync();
            var productIds = (await Task.WhenAll(
                _create.Product.WithStatusId(status.Id).WithName("Test1").BuildAsync(),
                _create.Product.WithStatusId(status.Id).WithName("Test2").BuildAsync()
            )).Select(x => x.Id).ToList();

            await _productsClient.DeleteAsync(productIds);

            var products = await _productsClient.GetListAsync(productIds);

            Assert.All(products, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            
            var status = await _create.ProductStatus.BuildAsync();
            var productIds = (await Task.WhenAll(
                    _create.Product.WithStatusId(status.Id).WithName("Test1").BuildAsync(),
                    _create.Product.WithStatusId(status.Id).WithName("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            await _productsClient.RestoreAsync(productIds);

            var products = await _productsClient.GetListAsync(productIds);

            Assert.All(products, x => Assert.False(x.IsDeleted));
        }
    }
}