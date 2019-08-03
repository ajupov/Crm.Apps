namespace Identity.OAuth.ViewModels
{
    public class AuthorizeViewModel
    {
        public AuthorizeViewModel(
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