using Identity.OAuth.Models.Authorize;

namespace Identity.OAuth.Models
{
    public class CallbackResponse
    {
        public CallbackResponse(
            string redirectUri)
        {
            RedirectUri = redirectUri;
        }

        public CallbackResponse(
            AuthorizeResponseError error)
        {
            Error = error;
        }

        public bool IsSuccess => Error.HasValue;

        public AuthorizeResponseError? Error { get; }

        public string RedirectUri { get; }
    }
}