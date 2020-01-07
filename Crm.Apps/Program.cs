using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.ApiDocumentation;
using Ajupov.Infrastructure.All.Hosting;
using Ajupov.Infrastructure.All.HotStorage;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Logging;
using Ajupov.Infrastructure.All.Metrics;
using Ajupov.Infrastructure.All.Migrations;
using Ajupov.Infrastructure.All.Mvc;
using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Tracing;
using Ajupov.Infrastructure.All.UserContext;
using Crm.Apps.Accounts.Services;
using Crm.Apps.Accounts.Storages;
using Crm.Apps.Activities.Services;
using Crm.Apps.Activities.Storages;
using Crm.Apps.Auth.Services;
using Crm.Apps.Auth.Settings;
using Crm.Apps.Companies.Services;
using Crm.Apps.Companies.Storages;
using Crm.Apps.Contacts.Services;
using Crm.Apps.Contacts.Storages;
using Crm.Apps.Deals.Services;
using Crm.Apps.Deals.Storages;
using Crm.Apps.Leads.Services;
using Crm.Apps.Leads.Storages;
using Crm.Apps.Products.Services;
using Crm.Apps.Products.Storages;
using Crm.Apps.Users.Services;
using Crm.Apps.Users.Storages;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ConfigurationExtensions = Ajupov.Infrastructure.All.Configuration.ConfigurationExtensions;

namespace Crm.Apps
{
    public static class Program
    {
        public static Task Main()
        {
            var configuration = ConfigurationExtensions.GetConfiguration();

            return configuration
                .ConfigureHost()
                .ConfigureLogging(configuration)
                .ConfigureServices((builder, services) =>
                {
                    services
                        .ConfigureJwtAuthentication(configuration)
                        .ConfigureJwtValidator(configuration)
                        .AddOAuth("LiteCRM Identity", options =>
                            {
                                var authSettings = configuration.GetSection("AuthSettings");

                                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                                options.ClientId = authSettings.GetValue<string>("ClientId");
                                options.ClientSecret = authSettings.GetValue<string>("ClientSecret");
                                options.AuthorizationEndpoint = authSettings.GetValue<string>("AuthorizationUrl");
                                options.TokenEndpoint = authSettings.GetValue<string>("TokenUrl");
                                options.CallbackPath = new PathString(authSettings.GetValue<string>("CallbackPath"));
                                options.Scope.Add(authSettings.GetValue<string>("Scope"));
                                options.SaveTokens = true;

                                // options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "nameidentifier");
                                // options.ClaimActions.MapJsonKey(ClaimTypes.Email, "emailaddress");
                                // options.ClaimActions.MapJsonKey(ClaimTypes.HomePhone, "homephone");
                                // options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "surname");
                                // options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                                // options.ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "dateofbirth");
                                // options.ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
                                // options.ClaimActions.MapJsonKey(ClaimTypes.Role, "role");

                                options.Events = new OAuthEvents
                                {
                                    OnCreatingTicket = context =>
                                    {
                                        // Move to utils
                                        var userDataPart = context.AccessToken.Split('.')[1];
                                        var bytes = Base64UrlTextEncoder.Decode(userDataPart);
                                        var userDataJson = Encoding.UTF8.GetString(bytes);
                                        var jsonElement = JsonDocument.Parse(userDataJson).RootElement;

                                        // Register if not registered
                                        // Save refresh token

                                        context.RunClaimActions(jsonElement);

                                        // context.Response.Cookies.Delete("AccessToken");
                                        var expiresInSeconds = int.Parse(context.TokenResponse.ExpiresIn);

                                        context.Response.Cookies.Append("AccessToken", context.AccessToken,
                                            new CookieOptions
                                            {
                                                // Domain = "http://localhost:9000",
                                                SameSite = SameSiteMode.None,
                                                // HttpOnly = true,
                                                Expires = DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds)
                                            });

                                        context.Response.Cookies.Append("AccessToken", context.AccessToken,
                                            new CookieOptions
                                            {
                                                // Domain = "http://localhost:3000",
                                                SameSite = SameSiteMode.None,
                                                // HttpOnly = true,
                                                Expires = DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds)
                                            });

                                        context.Response.Cookies.Append("AccessToken", context.AccessToken,
                                            new CookieOptions
                                            {
                                                // Domain = "http://litecrm.org",
                                                SameSite = SameSiteMode.None,
                                                // HttpOnly = true,
                                                Expires = DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds)
                                            });

                                        return Task.CompletedTask;
                                    }
                                };
                            }
                        );
                    services.AddAuthorization();
                    
                    services
                        .Configure<AuthSettings>(configuration.GetSection("AuthSettings"));

                    services
                        .ConfigureMvc()
                        .ConfigureTracing(configuration)
                        .ConfigureApiDocumentation()
                        .ConfigureMetrics(builder.Configuration)
                        .ConfigureMigrator(builder.Configuration)
                        .ConfigureOrm<AccountsStorage>(builder.Configuration)
                        .ConfigureOrm<UsersStorage>(builder.Configuration)
                        .ConfigureOrm<ProductsStorage>(builder.Configuration)
                        .ConfigureOrm<LeadsStorage>(builder.Configuration)
                        .ConfigureOrm<CompaniesStorage>(builder.Configuration)
                        .ConfigureOrm<ContactsStorage>(builder.Configuration)
                        .ConfigureOrm<DealsStorage>(builder.Configuration)
                        .ConfigureOrm<ActivitiesStorage>(builder.Configuration)
                        .ConfigureHotStorage(builder.Configuration)
                        .ConfigureUserContext<IUserContext, UserContext>()
                        .AddTransient<IAuthService, AuthService>()
                        .AddTransient<IAccountsService, AccountsService>()
                        .AddTransient<IAccountChangesService, AccountChangesService>()
                        .AddTransient<IUsersService, UsersService>()
                        .AddTransient<IUserChangesService, UserChangesService>()
                        .AddTransient<IUserAttributesService, UserAttributesService>()
                        .AddTransient<IUserAttributeChangesService, UserAttributeChangesService>()
                        .AddTransient<IUserGroupsService, UserGroupsService>()
                        .AddTransient<IUserGroupChangesService, UserGroupChangesService>()
                        .AddTransient<IProductsService, ProductsService>()
                        .AddTransient<IProductChangesService, ProductChangesService>()
                        .AddTransient<IProductCategoriesService, ProductCategoriesService>()
                        .AddTransient<IProductCategoryChangesService, ProductCategoryChangesService>()
                        .AddTransient<IProductStatusesService, ProductStatusesService>()
                        .AddTransient<IProductStatusChangesService, ProductStatusChangesService>()
                        .AddTransient<IProductAttributesService, ProductAttributesService>()
                        .AddTransient<IProductAttributeChangesService, ProductAttributeChangesService>()
                        .AddTransient<ILeadsService, LeadsService>()
                        .AddTransient<ILeadChangesService, LeadChangesService>()
                        .AddTransient<ILeadCommentsService, LeadCommentsService>()
                        .AddTransient<ILeadSourcesService, LeadSourcesService>()
                        .AddTransient<ILeadSourceChangesService, LeadSourceChangesService>()
                        .AddTransient<ILeadAttributesService, LeadAttributesService>()
                        .AddTransient<ILeadAttributeChangesService, LeadAttributeChangesService>()
                        .AddTransient<ICompaniesService, CompaniesService>()
                        .AddTransient<ICompanyChangesService, CompanyChangesService>()
                        .AddTransient<ICompanyAttributesService, CompanyAttributesService>()
                        .AddTransient<ICompanyAttributeChangesService, CompanyAttributeChangesService>()
                        .AddTransient<ICompanyCommentsService, CompanyCommentsService>()
                        .AddTransient<IContactsService, ContactsService>()
                        .AddTransient<IContactChangesService, ContactChangesService>()
                        .AddTransient<IContactAttributesService, ContactAttributesService>()
                        .AddTransient<IContactAttributeChangesService, ContactAttributeChangesService>()
                        .AddTransient<IContactCommentsService, ContactCommentsService>()
                        .AddTransient<IDealsService, DealsService>()
                        .AddTransient<IDealChangesService, DealChangesService>()
                        .AddTransient<IDealCommentsService, DealCommentsService>()
                        .AddTransient<IDealStatusesService, DealStatusesService>()
                        .AddTransient<IDealStatusChangesService, DealStatusChangesService>()
                        .AddTransient<IDealTypesService, DealTypesService>()
                        .AddTransient<IDealTypeChangesService, DealTypeChangesService>()
                        .AddTransient<IDealAttributesService, DealAttributesService>()
                        .AddTransient<IDealAttributeChangesService, DealAttributeChangesService>()
                        .AddTransient<IActivitiesService, ActivitiesService>()
                        .AddTransient<IActivityChangesService, ActivityChangesService>()
                        .AddTransient<IActivityCommentsService, ActivityCommentsService>()
                        .AddTransient<IActivityStatusesService, ActivityStatusesService>()
                        .AddTransient<IActivityStatusChangesService, ActivityStatusChangesService>()
                        .AddTransient<IActivityTypesService, ActivityTypesService>()
                        .AddTransient<IActivityTypeChangesService, ActivityTypeChangesService>()
                        .AddTransient<IActivityAttributesService, ActivityAttributesService>()
                        .AddTransient<IActivityAttributeChangesService, ActivityAttributeChangesService>();
                })
                .Configure((context, builder) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        builder.UseDeveloperExceptionPage();
                    }

                    builder
                        .UseApiDocumentationsMiddleware()
                        .UseMigrationsMiddleware()
                        .UseMetricsMiddleware()
                        .UseAuthentication()
                        .UseAuthorization()
                        .UseMvcMiddleware();
                })
                .Build()
                .RunAsync();
        }
    }
}