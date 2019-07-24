using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Identities.Parameters
{
    public class IsPasswordCorrectParameter
    {
        [Required] public Guid Id { get; set; }

        [Required] public string Password { get; set; }
    }
}