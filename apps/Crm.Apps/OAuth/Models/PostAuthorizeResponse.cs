namespace Crm.Apps.OAuth.Models
{
    public class PostAuthorizeResponse
    {
        public PostAuthorizeResponse(
            string redirectUri)
        {
            RedirectUri = redirectUri;
        }

        public PostAuthorizeResponse(
            AuthenticationResponseError error)
        {
            Error = error;
        }

        public bool IsSuccess => Error.HasValue;

        public AuthenticationResponseError? Error { get; }

        public string RedirectUri { get; }
    }
}