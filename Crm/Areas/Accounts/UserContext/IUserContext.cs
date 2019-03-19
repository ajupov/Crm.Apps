using System;

namespace Crm.Areas.Accounts.UserContext
{
    public interface IUserContext
    {
        Guid UserId { get; }
    }
}