using Ajupov.Infrastructure.All.Configuration;
using Ajupov.Infrastructure.All.Hosting;
using Ajupov.Infrastructure.All.Logging;
using Crm.Apps;
using Microsoft.AspNetCore.Hosting;

await Configuration
    .GetConfiguration()
    .ConfigureLogging()
    .ConfigureHosting<Startup>()
    .Build()
    .RunAsync();
