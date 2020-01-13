namespace Crm.Apps.Auth.Services
{
    public interface IAuthService
    {
        string GetCorrectRedirectUri(string redirectUri);
    }
}