using System.ComponentModel.DataAnnotations;

namespace Crm.Models.Registration
{
    public class ConfirmEmailRequestModel
    {
        [Required]
        public int IdentityId { get; set; }

        [Required, DataType(DataType.Text), StringLength(256)]
        public string Code { get; set; }
    }
}