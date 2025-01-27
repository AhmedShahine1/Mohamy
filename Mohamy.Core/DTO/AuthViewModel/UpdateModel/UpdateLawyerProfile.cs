using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Mohamy.Core.DTO.AuthViewModel.UpdateModel
{
    public class UpdateLawyerProfile
    {
        [DisplayName("Description")]
        [Required(ErrorMessage = "You should enter description")]
        public string Description { get; set; }

        [DisplayName("YearsExperience")]
        [Required(ErrorMessage = "Please enter year of experience")]
        public int YearsExperience { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "You should select city")]
        public string City { get; set; }

        [DisplayName("LicenseNumber")]
        [Required(ErrorMessage = "You should enter license number")]
        public string LicenseNumber { get; set; }

        [DisplayName("AcademicQualification")]
        [Required(ErrorMessage = "You should enter academic qualification")]
        public string? AcademicQualification { get; set; }

        [Required(ErrorMessage = "You should enter profession")]
        public List<CreateProfessionDto> Professions { get; set; }
    }
}
