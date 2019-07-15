using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Infrastructure.OAuthClients
{
    public static class OAuthExtensions
    {
        public static IServiceCollection ConfigureOAuthClients(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication()
                .AddVkontakte(x =>
                {
                    var section = configuration.GetSection("VkontakteOauthClientSettings");

                    x.ClientId = section.GetValue<string>("ClientId");
                    x.ClientSecret = section.GetValue<string>("ClientSecret");
                    x.CallbackPath = "/signin-vkontakte";
                })
                .AddOdnoklassniki(x =>
                {
                    var section = configuration.GetSection("OdnoklassnikiOauthClientSettings");

                    x.ClientId = section.GetValue<string>("ClientId");
                    x.ClientSecret = section.GetValue<string>("ClientSecret");
                    x.CallbackPath = "/signin-odnoklassniki";
//                    x.Scope.Add("VALUABLE_ACCESS");
//                    x.Scope.Add("LONG_ACCESS_TOKEN");
                })
                .AddInstagram(x =>
                {
                    var section = configuration.GetSection("InstagramOauthClientSettings");

                    x.ClientId = section.GetValue<string>("ClientId");
                    x.ClientSecret = section.GetValue<string>("ClientSecret");
                    x.CallbackPath = "/signin-instagram";
                })
                .AddYandex(x =>
                {
                    var section = configuration.GetSection("YandexOauthClientSettings");

                    x.ClientId = section.GetValue<string>("ClientId");
                    x.ClientSecret = section.GetValue<string>("ClientSecret");
                    x.CallbackPath = "/signin-yandex";
                })
                .AddMailRu(x =>
                {
                    var section = configuration.GetSection("MailRuOauthClientSettings");

                    x.ClientId = section.GetValue<string>("ClientId");
                    x.ClientSecret = section.GetValue<string>("ClientSecret");
                    x.CallbackPath = "/signin-mailru";
                });

            return services;
        }

        public static IApplicationBuilder UseOAuthClients(
            this IApplicationBuilder app)
        {
            return app.UseAuthentication();
        }
    }
}