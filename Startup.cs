using Ajupov.Infrastructure.All.Api;
using Ajupov.Infrastructure.All.ApiDocumentation;
using Ajupov.Infrastructure.All.Cookies;
using Ajupov.Infrastructure.All.Cors;
using Ajupov.Infrastructure.All.HotStorage;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Metrics;
using Ajupov.Infrastructure.All.Migrations;
using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Tracing;
using Ajupov.Infrastructure.All.UserContext;
using Crm.Apps.Account.Services;
using Crm.Apps.Account.Storages;
using Crm.Apps.Auth.Settings;
using Crm.Apps.Customers.Services;
using Crm.Apps.Customers.Storages;
using Crm.Apps.Orders.Services;
using Crm.Apps.Orders.Storages;
using Crm.Apps.Products.Services;
using Crm.Apps.Products.Storages;
using Crm.Apps.Stock.Services;
using Crm.Apps.Stock.Storages;
using Crm.Apps.Suppliers.Services;
using Crm.Apps.Suppliers.Storages;
using Crm.Apps.Tasks.Services;
using Crm.Apps.Tasks.Storages;
using Crm.Apps.User.Services;
using Crm.Apps.User.Storages;
using Crm.Common.All.UserContext;
using LiteCrm.OAuth.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthorization()
                .AddJwtAuthentication()
                .AddJwtValidator(Configuration)
                .AddLiteCrmOAuth(Configuration)
                .AddCookieDefaults();

            services
                .AddCookiePolicy()
                .AddSingleOriginCorsPolicy(Configuration)
                .AddApiControllers()
                .AddTracing(Configuration)
                .AddApiDocumentation()
                .AddMetrics(Configuration)
                .AddMigrator(Configuration)
                .AddOrm<AccountStorage>(Configuration)
                .AddOrm<UserStorage>(Configuration)
                .AddOrm<ProductsStorage>(Configuration)
                .AddOrm<CustomersStorage>(Configuration)
                .AddOrm<OrdersStorage>(Configuration)
                .AddOrm<TasksStorage>(Configuration)
                .AddOrm<SuppliersStorage>(Configuration)
                .AddOrm<StockStorage>(Configuration)
                .AddUserContext<IUserContext, UserContext>()
                .AddHotStorage(Configuration);

            services
                .Configure<AuthSettings>(Configuration.GetSection(nameof(AuthSettings)));

            services
                .AddTransient<IAccountFlagsService, AccountFlagsService>()
                .AddTransient<IUserFlagsService, UserFlagsService>()
                .AddTransient<IAccountSettingsService, AccountSettingsService>()
                .AddTransient<IAccountSettingChangesService, AccountSettingChangesService>()
                .AddTransient<IUserSettingsService, UserSettingsService>()
                .AddTransient<IUserSettingChangesService, UserSettingChangesService>()
                .AddTransient<IProductsService, ProductsService>()
                .AddTransient<IProductChangesService, ProductChangesService>()
                .AddTransient<IProductCategoriesService, ProductCategoriesService>()
                .AddTransient<IProductCategoryChangesService, ProductCategoryChangesService>()
                .AddTransient<IProductStatusesService, ProductStatusesService>()
                .AddTransient<IProductStatusChangesService, ProductStatusChangesService>()
                .AddTransient<IProductAttributesService, ProductAttributesService>()
                .AddTransient<IProductAttributeChangesService, ProductAttributeChangesService>()
                .AddTransient<ICustomersService, CustomersService>()
                .AddTransient<ICustomerChangesService, CustomerChangesService>()
                .AddTransient<ICustomerCommentsService, CustomerCommentsService>()
                .AddTransient<ICustomerSourcesService, CustomerSourcesService>()
                .AddTransient<ICustomerSourceChangesService, CustomerSourceChangesService>()
                .AddTransient<ICustomerAttributesService, CustomerAttributesService>()
                .AddTransient<ICustomerAttributeChangesService, CustomerAttributeChangesService>()
                .AddTransient<IOrdersService, OrdersService>()
                .AddTransient<IOrderChangesService, OrderChangesService>()
                .AddTransient<IOrderCommentsService, OrderCommentsService>()
                .AddTransient<IOrderStatusesService, OrderStatusesService>()
                .AddTransient<IOrderStatusChangesService, OrderStatusChangesService>()
                .AddTransient<IOrderTypesService, OrderTypesService>()
                .AddTransient<IOrderTypeChangesService, OrderTypeChangesService>()
                .AddTransient<IOrderAttributesService, OrderAttributesService>()
                .AddTransient<IOrderAttributeChangesService, OrderAttributeChangesService>()
                .AddTransient<ITasksService, TasksService>()
                .AddTransient<ITaskChangesService, TaskChangesService>()
                .AddTransient<ITaskCommentsService, TaskCommentsService>()
                .AddTransient<ITaskStatusesService, TaskStatusesService>()
                .AddTransient<ITaskStatusChangesService, TaskStatusChangesService>()
                .AddTransient<ITaskTypesService, TaskTypesService>()
                .AddTransient<ITaskTypeChangesService, TaskTypeChangesService>()
                .AddTransient<ITaskAttributesService, TaskAttributesService>()
                .AddTransient<ITaskAttributeChangesService, TaskAttributeChangesService>()
                .AddTransient<ISuppliersService, SuppliersService>()
                .AddTransient<ISupplierChangesService, SupplierChangesService>()
                .AddTransient<ISupplierCommentsService, SupplierCommentsService>()
                .AddTransient<ISupplierAttributesService, SupplierAttributesService>()
                .AddTransient<ISupplierAttributeChangesService, SupplierAttributeChangesService>()
                .AddTransient<IStockArrivalsService, StockArrivalsService>()
                .AddTransient<IStockArrivalChangesService, StockArrivalChangesService>()
                .AddTransient<IStockBalancesService, StockBalancesService>()
                .AddTransient<IStockBalanceChangesService, StockBalanceChangesService>()
                .AddTransient<IStockConsumptionsService, StockConsumptionsService>()
                .AddTransient<IStockConsumptionChangesService, StockConsumptionChangesService>()
                .AddTransient<IStockRoomsService, StockRoomsService>()
                .AddTransient<IStockRoomChangesService, StockRoomChangesService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseForwardedHeaders()
                    .UseHttpsRedirection()
                    .UseHsts();
            }

            app.UseApiDocumentationsMiddleware()
                .UseMigrationsMiddleware()
                .UseMetricsMiddleware()
                .UseCookiePolicy()
                .UseSingleOriginCors()
                .UseAuthentication()
                .UseRouting()
                .UseAuthorization()
                .UseControllers();
        }
    }
}
