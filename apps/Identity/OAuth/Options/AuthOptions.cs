using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Identity.OAuth.Options
{
    public class AuthOptions
    {
        public const string ISSUER = "Identity"; // издатель токена
        public const string AUDIENCE = "http://localhost:51884/"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int LIFETIME = 1; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}