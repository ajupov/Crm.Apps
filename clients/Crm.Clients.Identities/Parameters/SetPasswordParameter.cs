using System;

namespace Crm.Clients.Identities.Parameters
{
    public class SetPasswordParameter
    {
        public SetPasswordParameter(
            Guid id,
            string password)
        {
            Id = id;
            Password = password;
        }

        public Guid Id { get; }

        public string Password { get; }
    }
}