using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Mohamy.Core.DTO.AuthViewModel.UpdateModel
{
    public class UpdatePassword
    {
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "You should enter the Password")]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "You should confirm the Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation do not match")]
        public string ConfirmPassword { get; set; }
    }
}
