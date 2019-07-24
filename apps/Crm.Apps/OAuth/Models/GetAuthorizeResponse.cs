namespace Crm.Apps.OAuth.Models
{
    public class GetAuthorizeResponse
    {
        public GetAuthorizeResponse(
            string responseType,
            string redirectUri)
        {
            ResponseType = responseType;
            RedirectUri = redirectUri;
        }

        public string ResponseType { get; }

        public string RedirectUri { get; }
    }
}