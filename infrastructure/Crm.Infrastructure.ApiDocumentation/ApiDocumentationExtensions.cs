using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Crm.Infrastructure.ApiDocumentation
{
    public static class ApiDocumentationExtensions
    {
        public static IServiceCollection ConfigureApiDocumentation(this IServiceCollection services,
            string applicationName, string apiVersion)
        {
            var info = new Info
            {
                Title = applicationName,
                Version = apiVersion
            };

            return services
                .AddSwaggerGen(options => options.SwaggerDoc(apiVersion, info));
        }

        public static IApplicationBuilder UseApiDocumentationsMiddleware(this IApplicationBuilder applicationBuilder,
            string applicationName, string apiVersion)
        {
            var url = $"/swagger/{apiVersion}/swagger.json";

            return applicationBuilder.UseSwagger()
                .UseSwaggerUI(options => options.SwaggerEndpoint(url, applicationName));
        }
    }
}