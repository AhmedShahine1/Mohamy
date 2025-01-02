using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Mohamy.Core.DTO.AuthViewModel.UpdateModel
{
    public class LawyerInitialDetail
    {
        [DisplayName("Descriptiion")]
        [Required(ErrorMessage = "You should enter the Description")]
        public string Description { get; set; }

        [DisplayName("PriceService")]
        [Required(ErrorMessage = "You should enter Price")]
        public double? PriceService { get; set; }

        [DisplayName("YearsExperience")]
        [Required(ErrorMessage = "You should enter Experience")]
        public int YearsExperience { get; set; }

        [DisplayName("Profile Image")]
        public IFormFile? ImageProfile { get; set; }
    }
}
