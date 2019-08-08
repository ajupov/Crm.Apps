using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;

namespace Identity.Registration.Models
{
    public class RegisterResponseModel
    {
        public RegisterResponseModel(string[] errors)
        {
            Errors = errors;
        }

        public string[] Errors { get; set; }

        public bool IsSuccess => Errors == null || !Errors.Any();
    }
}