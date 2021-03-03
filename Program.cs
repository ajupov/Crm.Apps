using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Configuration;
using Ajupov.Infrastructure.All.Hosting;
using Ajupov.Infrastructure.All.Logging;
using Microsoft.AspNetCore.Hosting;

namespace Crm.Apps
{
    public static class Program
    {
        public static Task Main()
        {
            var configuration = Configuration.GetConfiguration();

            return configuration
                .ConfigureHosting<Startup>()
                .ConfigureLogging(configuration)
                .Build()
                .RunAsync();
        }
    }
}
