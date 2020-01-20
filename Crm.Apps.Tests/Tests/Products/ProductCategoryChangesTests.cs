using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Json;
using Crm.Apps.Clients.Products.Clients;
using Crm.Apps.Clients.Products.Models;
using Crm.Apps.Tests.Creator;
using Xunit;

namespace Crm.Apps.Tests.Tests.Products
{
    public class ProductCategoryChangesTests
    {
        private readonly ICreate _create;
        private readonly IProductCategoriesClient _productCategoriesClient;
        private readonly IProductCategoryChangesClient _groupChangesClient;

        public ProductCategoryChangesTests(ICreate create, IProductCategoriesClient productCategoriesClient,
            IProductCategoryChangesClient groupChangesClient)
        {
            _create = create;
            _productCategoriesClient = productCategoriesClient;
            _groupChangesClient = groupChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            
            var category = await _create.ProductCategory.BuildAsync();
            category.Name = "Test2";
            category.IsDeleted = true;
            await _productCategoriesClient.UpdateAsync(category);

            var changes = await _groupChangesClient
                .GetPagedListAsync(categoryId: category.Id, sortBy: "CreateDateTime", orderBy: "asc")
                ;

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.CategoryId == category.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<ProductCategory>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<ProductCategory>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<ProductCategory>().IsDeleted);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<ProductCategory>().Name, category.Name);
        }
    }
}