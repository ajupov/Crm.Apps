using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Common.OAuth.Odnoklassniki
{
    public static class OdnoklassnikiAuthenticationExtensions
    {
        public static AuthenticationBuilder AddOdnoklassniki(this AuthenticationBuilder builder)
        {
            return builder.AddOdnoklassniki(OdnoklassnikiAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        public static AuthenticationBuilder AddOdnoklassniki(this AuthenticationBuilder builder,
            Action<OdnoklassnikiAuthenticationOptions> configuration)
        {
            return builder.AddOdnoklassniki(OdnoklassnikiAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        public static AuthenticationBuilder AddOdnoklassniki(this AuthenticationBuilder builder,
            string scheme, Action<OdnoklassnikiAuthenticationOptions> configuration)
        {
            return builder.AddOdnoklassniki(scheme, OdnoklassnikiAuthenticationDefaults.DisplayName, configuration);
        }

        public static AuthenticationBuilder AddOdnoklassniki(this AuthenticationBuilder builder,
            string scheme, string caption, Action<OdnoklassnikiAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<OdnoklassnikiAuthenticationOptions, OdnoklassnikiAuthenticationHandler>(
                scheme, caption, configuration);
        }
    }
}