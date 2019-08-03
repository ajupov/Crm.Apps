using System.Linq;
using Identity.Identities.Models;

namespace Identity.Identities.Extensions
{
    public static class IdentityTypeExtensions
    {
        public static readonly IdentityType[] TypesWithPassword =
        {
            IdentityType.LoginAndPassword,
            IdentityType.EmailAndPassword,
            IdentityType.PhoneAndPassword
        };

        public static bool IsTypeWithPassword(this IdentityType type)
        {
            return TypesWithPassword.ToList().Contains(type);
        }
    }
}