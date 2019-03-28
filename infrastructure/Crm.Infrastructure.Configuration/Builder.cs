using System;
using Microsoft.Extensions.Configuration;

namespace Crm.Infrastructure.Configuration
{
    public static class Builder
    {
        public static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}