using System.Threading.Tasks;
using Crm.Areas.Accounts.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Crm
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureSerilog()
                .UseStartup<Startup>()
                .Build()
                .RunAsync();
        }
    }
}