using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Products.Clients;
using Crm.Clients.Products.Models;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Tests.Products
{
    public class ProductCategoriesTests
    {
        private readonly ICreate _create;
        private readonly IProductCategoriesClient _productCategoriesClient;

        public ProductCategoriesTests(ICreate create, IProductCategoriesClient productCategoriesClient)
        {
            _create = create;
            _productCategoriesClient = productCategoriesClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var categoriesId =
                (await _create.ProductCategory.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false)).Id;

            var categories = await _productCategoriesClient.GetAsync(categoriesId).ConfigureAwait(false);

            Assert.NotNull(categories);
            Assert.Equal(categoriesId, categories.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var categoriesIds = (await Task.WhenAll(
                    _create.ProductCategory.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.ProductCategory.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            var categories = await _productCategoriesClient.GetListAsync(categoriesIds).ConfigureAwait(false);

            Assert.NotEmpty(categories);
            Assert.Equal(categoriesIds.Count, categories.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            await Task.WhenAll(_create.ProductCategory.WithAccountId(account.Id).WithName("Test1").BuildAsync())
                .ConfigureAwait(false);

            var categories = await _productCategoriesClient
                .GetPagedListAsync(account.Id, "Test1", sortBy: "CreateDateTime", orderBy: "desc")
                .ConfigureAwait(false);

            var results = categories.Skip(1).Zip(categories,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(categories);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var categories = new ProductCategory
            {
                AccountId = account.Id,
                Name = "Test",
                IsDeleted = false
            };

            var createdCategoryId = await _productCategoriesClient.CreateAsync(categories).ConfigureAwait(false);

            var createdCategory = await _productCategoriesClient.GetAsync(createdCategoryId).ConfigureAwait(false);

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
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var categories = await _create.ProductCategory.WithAccountId(account.Id).WithName("Test1").BuildAsync()
                .ConfigureAwait(false);

            categories.Name = "Test2";
            categories.IsDeleted = true;

            await _productCategoriesClient.UpdateAsync(categories).ConfigureAwait(false);

            var updatedCategory = await _productCategoriesClient.GetAsync(categories.Id).ConfigureAwait(false);

            Assert.Equal(categories.Name, updatedCategory.Name);
            Assert.Equal(categories.IsDeleted, updatedCategory.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var categoriesIds = (await Task.WhenAll(
                    _create.ProductCategory.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.ProductCategory.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _productCategoriesClient.DeleteAsync(categoriesIds).ConfigureAwait(false);

            var categories = await _productCategoriesClient.GetListAsync(categoriesIds).ConfigureAwait(false);

            Assert.All(categories, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var categoriesIds = (await Task.WhenAll(
                    _create.ProductCategory.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.ProductCategory.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _productCategoriesClient.RestoreAsync(categoriesIds).ConfigureAwait(false);

            var categories = await _productCategoriesClient.GetListAsync(categoriesIds).ConfigureAwait(false);

            Assert.All(categories, x => Assert.False(x.IsDeleted));
        }
    }
}