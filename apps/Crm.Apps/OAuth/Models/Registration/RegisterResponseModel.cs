using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;

namespace Crm.Apps.OAuth.Models.Registration
{
    public class RegisterResponseModel
    {
        public List<string> Errors { get; set; }

        public bool IsSuccess => Errors == null || !Errors.Any();
    }
}