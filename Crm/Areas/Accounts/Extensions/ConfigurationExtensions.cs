using System.Reflection;
using Crm.Areas.Accounts.Configs;
using Crm.Areas.Accounts.Consumers;
using Crm.Areas.Accounts.Services;
using Crm.Areas.Accounts.Storages;
using Crm.Areas.Accounts.UserContext;
using Crm.Infrastructure.Migrations;
using Crm.Infrastructure.Orm;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crm.Areas.Accounts.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration)
                .AddOptions()
                .Configure<DbSqlSettings>(settings => configuration.GetSection("DbSql").Bind(settings))
                .Configure<MbKafkaSettings>(settings => configuration.GetSection("MbKafka").Bind(settings))
                .ConfigureMigrator(configuration.GetConnectionString("DbSqlMain"), Assembly.GetExecutingAssembly())
                .ConfigureOrm<AccountsStorage>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<IAccountsService, AccountsService>()
                .AddSingleton<IHostedService, AccountsConsumer>()
                .AddScoped<IUserContext, UserContext.UserContext>()
                .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public static void Configure(this IApplicationBuilder builder)
        {
            builder.Migrate();
            builder.UseHttpsRedirection();
            builder.UseMvc();
        }
    }
}