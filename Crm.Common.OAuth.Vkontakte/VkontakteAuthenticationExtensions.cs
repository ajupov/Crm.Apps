using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Libs.OAuth.Vkontakte
{
    public static class VkontakteAuthenticationExtensions
    {
        public static AuthenticationBuilder AddVkontakte(this AuthenticationBuilder builder)
        {
            return builder.AddVkontakte(VkontakteAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        public static AuthenticationBuilder AddVkontakte(
            this AuthenticationBuilder builder,
            Action<VkontakteAuthenticationOptions> configuration)
        {
            return builder.AddVkontakte(VkontakteAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        public static AuthenticationBuilder AddVkontakte(
            this AuthenticationBuilder builder, string scheme,
            Action<VkontakteAuthenticationOptions> configuration)
        {
            return builder.AddVkontakte(scheme, VkontakteAuthenticationDefaults.DisplayName, configuration);
        }

        public static AuthenticationBuilder AddVkontakte(
            this AuthenticationBuilder builder,
            string scheme, string caption,
            Action<VkontakteAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<VkontakteAuthenticationOptions, VkontakteAuthenticationHandler>(scheme, caption,
                configuration);
        }
    }
}