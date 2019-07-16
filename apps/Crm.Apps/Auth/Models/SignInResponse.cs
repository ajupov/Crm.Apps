using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;

namespace Crm.Apps.Auth.Models
{
    public class SignInResponse
    {
        public SignInResponse(params string[] errors)
        {
            Errors = errors.ToList();
        }

        public List<string> Errors { get; set; }

        public bool IsSuccess => Errors == null || !EnumerableExtensions.Any(Errors);
    }
}