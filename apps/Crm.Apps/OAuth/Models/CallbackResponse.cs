namespace Crm.Apps.OAuth.Models
{
    public class CallbackResponse
    {
        public CallbackResponse(
            string redirectUri)
        {
            RedirectUri = redirectUri;
        }

        public CallbackResponse(
            AuthenticationResponseError error)
        {
            Error = error;
        }

        public bool IsSuccess => Error.HasValue;

        public AuthenticationResponseError? Error { get; }

        public string RedirectUri { get; }
    }
}