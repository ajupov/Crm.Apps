﻿using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Auth.Models
{
    public class CallbackRequest
    {
        public CallbackRequest(
            string state,
            string redirectUrl)
        {
            State = state;
            RedirectUrl = redirectUrl;
        }

        [Required]
//        [StateValidation]
        public string State { get; }

        [StringLength(2048)]
        [DataType(DataType.Url)]
        public string RedirectUrl { get; }
    }
}