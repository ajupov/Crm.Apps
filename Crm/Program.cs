using System.Threading.Tasks;
using Crm.Areas.Accounts.Extensions;
using Crm.Infrastructure.Configuration;
using Crm.Infrastructure.Logging;
using Microsoft.AspNetCore.Hosting;

namespace Crm
{
    public class Program
    {
        public static Task Main()
        {
            return new WebHostBuilder()
                .ConfigureAppConfiguration(builder => builder.ConfigureAppConfiguration())
                .ConfigureLogging(builder => builder.ConfigureLogging())
                .ConfigureServices((builder, services) => services.ConfigureServices(builder.Configuration))
                .Configure(builder => builder.Configure())
                .UseKestrel()
                .Build()
                .RunAsync();
        }
    }
}