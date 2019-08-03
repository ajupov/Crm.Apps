namespace Identity.OAuth.Models.Authorize
{
    public class PostAuthorizeResponse
    {
        public PostAuthorizeResponse(string redirectUri)
        {
            RedirectUri = redirectUri;
        }

        public PostAuthorizeResponse(AuthorizeResponseError error)
        {
            Error = error;
        }

        public bool IsSuccess => Error.HasValue;

        public AuthorizeResponseError? Error { get; }

        public string RedirectUri { get; }
    }
}