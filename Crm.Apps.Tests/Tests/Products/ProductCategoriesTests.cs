using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.Tests.Services.Creator;
using Crm.Apps.v1.Clients.Products.Clients;
using Crm.Apps.v1.Clients.Products.Models;
using Crm.Apps.v1.Clients.Products.RequestParameters;
using Xunit;

namespace Crm.Apps.Tests.Tests.Products
{
    public class ProductCategoriesTests
    {
        private readonly IAccessTokenGetter _accessTokenGetter;
        private readonly ICreate _create;
        private readonly IProductCategoriesClient _productCategoriesClient;

        public ProductCategoriesTests(
            IAccessTokenGetter accessTokenGetter,
            ICreate create,
            IProductCategoriesClient productCategoriesClient)
        {
            _accessTokenGetter = accessTokenGetter;
            _create = create;
            _productCategoriesClient = productCategoriesClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var categoriesId = (await _create.ProductCategory.BuildAsync()).Id;

            var categories = await _productCategoriesClient.GetAsync(accessToken, categoriesId);

            Assert.NotNull(categories);
            Assert.Equal(categoriesId, categories.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var categoriesIds = (
                    await Task.WhenAll(
                        _create.ProductCategory
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.ProductCategory
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            var categories = await _productCategoriesClient.GetListAsync(accessToken, categoriesIds);

            Assert.NotEmpty(categories);
            Assert.Equal(categoriesIds.Count, categories.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            await Task.WhenAll(_create.ProductCategory
                .WithName("Test1")
                .BuildAsync());

            var request = new ProductCategoryGetPagedListRequestParameter
            {
                Name = "Test1"
            };

            var categories = await _productCategoriesClient.GetPagedListAsync(accessToken, request);

            var results = categories
                .Skip(1)
                .Zip(categories, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(categories);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var categories = new ProductCategory
            {
                Name = "Test",
                IsDeleted = false
            };

            var createdCategoryId = await _productCategoriesClient.CreateAsync(accessToken, categories);

            var createdCategory = await _productCategoriesClient.GetAsync(accessToken, createdCategoryId);

            Assert.NotNull(createdCategory);
            Assert.Equal(createdCategoryId, createdCategory.Id);
            Assert.Equal(categories.AccountId, createdCategory.AccountId);
            Assert.Equal(categories.Name, createdCategory.Name);
            Assert.Equal(categories.IsDeleted, createdCategory.IsDeleted);
            Assert.True(createdCategory.CreateDateTime.IsMoreThanMinValue());
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var categories = await _create.ProductCategory
                .WithName("Test1")
                .BuildAsync();

            categories.Name = "Test2";
            categories.IsDeleted = true;

            await _productCategoriesClient.UpdateAsync(accessToken, categories);

            var updatedCategory = await _productCategoriesClient.GetAsync(accessToken, categories.Id);

            Assert.Equal(categories.Name, updatedCategory.Name);
            Assert.Equal(categories.IsDeleted, updatedCategory.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var categoriesIds = (
                    await Task.WhenAll(
                        _create.ProductCategory
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.ProductCategory
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _productCategoriesClient.DeleteAsync(accessToken, categoriesIds);

            var categories = await _productCategoriesClient.GetListAsync(accessToken, categoriesIds);

            Assert.All(categories, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var categoriesIds = (
                    await Task.WhenAll(
                        _create.ProductCategory
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.ProductCategory
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _productCategoriesClient.RestoreAsync(accessToken, categoriesIds);

            var categories = await _productCategoriesClient.GetListAsync(accessToken, categoriesIds);

            Assert.All(categories, x => Assert.False(x.IsDeleted));
        }
    }
}