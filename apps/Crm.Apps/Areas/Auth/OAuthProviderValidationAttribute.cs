using System.ComponentModel.DataAnnotations;
using Crm.Apps.Areas.Auth.Models;

namespace Crm.Apps.Areas.Auth
{
    public class OAuthProviderValidationAttribute : ValidationAttribute
    {
        private const string TranslationsKey = "OAuth:Errors:Validation";

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            var oauthProvider = value?.ToString();
            if (oauthProvider == OAuthProvider.Crm ||
                oauthProvider == OAuthProvider.Vkontakte ||
                oauthProvider == OAuthProvider.Odnoklassniki ||
                oauthProvider == OAuthProvider.Instagram ||
                oauthProvider == OAuthProvider.Yandex ||
                oauthProvider == OAuthProvider.MailRu)
            {
                return new ValidationResult($"{TranslationsKey}_Provider:IsInvalid");
            }

            return ValidationResult.Success;
        }
    }
}