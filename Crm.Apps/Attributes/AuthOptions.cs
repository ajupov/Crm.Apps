using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Crm.Apps.Attributes
{
    public class AuthOptions
    {
        public const string Issuer = "Identity";
        public const string Audience = "LiteCRM";
        public const string CrmIdentityAuthenticationScheme = "LiteCRM";
        private const string Key = "VeryLargestSecretKey";

        public static SymmetricSecurityKey GetKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}