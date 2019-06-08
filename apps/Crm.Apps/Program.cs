﻿using System.Threading.Tasks;
using Crm.Apps.Accounts.Consumers;
using Crm.Apps.Accounts.Services;
using Crm.Apps.Accounts.Storages;
using Crm.Apps.Identities.Services;
using Crm.Apps.Identities.Storages;
using Crm.Apps.Users.Consumers;
using Crm.Apps.Users.Services;
using Crm.Apps.Users.Storages;
using Crm.Common.UserContext;
using Crm.Infrastructure.ApiDocumentation;
using Crm.Infrastructure.Configuration;
using Crm.Infrastructure.Hosting;
using Crm.Infrastructure.Logging;
using Crm.Infrastructure.MessageBroking;
using Crm.Infrastructure.Metrics;
using Crm.Infrastructure.Migrations;
using Crm.Infrastructure.Mvc;
using Crm.Infrastructure.Orm;
using Crm.Infrastructure.Tracing;
using Crm.Infrastructure.UserContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Apps
{
    public static class Program
    {
        private const string ApplicationName = "Crm";
        private const string ApplicationVersion = "v1";

        public static Task Main()
        {
            return
                ConfigurationExtensions.GetConfiguration()
                    .ConfigureHost()
                    .ConfigureLogging(ApplicationName, ApplicationVersion)
                    .ConfigureServices((builder, services) =>
                    {
                        var configuration = builder.Configuration;

                        services
                            .ConfigureApiDocumentation(ApplicationName, ApplicationVersion)
                            .ConfigureMetrics(configuration)
                            .ConfigureTracing(ApplicationName)
                            .ConfigureMigrator(configuration)
                            .ConfigureOrm<AccountsStorage>(configuration)
                            .ConfigureOrm<UsersStorage>(configuration)
                            .ConfigureOrm<IdentitiesStorage>(configuration)
                            .ConfigureConsumer<AccountsConsumer>(configuration)
                            .ConfigureConsumer<UsersConsumer>(configuration)
                            .ConfigureConsumer<UserAttributesConsumer>(configuration)
                            .ConfigureConsumer<UserGroupsConsumer>(configuration)
                            .ConfigureMvc()
                            .ConfigureUserContext<IUserContext, UserContext>()
                            .AddTransient<IAccountsService, AccountsService>()
                            .AddTransient<IAccountChangesService, AccountChangesService>()
                            .AddTransient<IUsersService, UsersService>()
                            .AddTransient<IUserChangesService, UserChangesService>()
                            .AddTransient<IUserAttributesService, UserAttributesService>()
                            .AddTransient<IUserAttributeChangesService, UserAttributeChangesService>()
                            .AddTransient<IUserGroupsService, UserGroupsService>()
                            .AddTransient<IUserGroupChangesService, UserGroupChangesService>()
                            .AddTransient<IIdentitiesService, IdentitiesService>()
                            .AddTransient<IIdentityTokensService, IdentityTokensService>();
                    })
                    .Configure(builder => builder
                        .UseApiDocumentationsMiddleware(ApplicationName, ApplicationVersion)
                        .UseMigrationsMiddleware()
                        .UseMetricsMiddleware()
                        .UseMvcMiddleware())
                    .Build()
                    .RunAsync();
        }
    }
}