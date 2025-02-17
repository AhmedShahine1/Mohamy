using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mohamy.Core.DTO.AuthViewModel.RegisterModel
{
    public class RegisterCustomer
    {
        [DisplayName("Full Name")]
        [Required(ErrorMessage = "You should enter the Full Name"), StringLength(100)]
        public string FullName { get; set; }

        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "You should enter the Phone Number"), StringLength(15)]
        public string PhoneNumber { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "You should select city")]
        public string City { get; set; }

        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DisplayName("Profile Image")]
        public IFormFile? ImageProfile { get; set; }
    }
}
