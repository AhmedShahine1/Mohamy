using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace Mohamy.Core.DTO.AuthViewModel.UpdateModel
{
    public class UpdateProfileImage
    {
        [DisplayName("Profile Image")]
        public IFormFile ImageProfile { get; set; }
    }
}
