using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mohamy.Core.DTO.AuthViewModel.RegisterModel
{
    public class RegisterLawyer
    {
        [DisplayName("Full Name")]
        [Required(ErrorMessage = "Full Name field is required"), StringLength(100)]
        public string FullName { get; set; }

        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number field is required"), StringLength(15)]
        public string PhoneNumber { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Email field is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "City field is required"), StringLength(100)]
        public string City { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Password field is required")]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Confirm Password field is required")]
        public string ConfirmPassword { get; set; }


        [DisplayName("License Number")]
        [Required(ErrorMessage = "License Number field is required"), StringLength(100)]
        public string LicenseNumber { get; set; }

        [DisplayName("Licenses")]
        public List<IFormFile> Licenses { get; set; }

        [DisplayName("Certificates")]
        public List<IFormFile> Certificates { get; set; }
    }
}
