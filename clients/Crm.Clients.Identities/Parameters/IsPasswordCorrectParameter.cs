namespace Crm.Clients.Identities.Parameters
{
    public class IsPasswordCorrectParameter
    {
        public IsPasswordCorrectParameter(
            string key,
            string password)
        {
            Id = key;
            Password = password;
        }

        public string Id { get; }

        public string Password { get; }
    }
}