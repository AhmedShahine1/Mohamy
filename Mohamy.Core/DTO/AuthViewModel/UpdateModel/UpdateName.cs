using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Mohamy.Core.DTO.AuthViewModel.UpdateModel
{
    public class UpdateLawyer
    {
        [DisplayName("Full Name")]
        [Required(ErrorMessage = "You should enter the Full Name"), StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "You should enter the Password")]
        public string Password { get; set; }
    }
}
