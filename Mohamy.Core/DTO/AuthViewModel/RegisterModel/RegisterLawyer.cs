using Microsoft.AspNetCore.Http;
using Mohamy.Core.DTO.AuthViewModel.LawyerDetailsModel;
using System.ComponentModel;

namespace Mohamy.Core.DTO.AuthViewModel.RegisterModel
{
    public class RegisterLawyer
    {
        // Lawyer basic info
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }
        public int? YearsExperience { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }

        // Lawyer banking details
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? IBAN { get; set; }

        // Lawyer license
        public string LicenseNumber { get; set; }
        public DateTime LicenseStart { get; set; }
        public DateTime LicenseEnd { get; set; }
        public string State { get; set; }

        [DisplayName("الصورة الشخصية")]
        public IFormFile? ImageProfile { get; set; }

        // Graduation certificates
        public List<GraduationCertificateDTO> GraduationCertificates { get; set; } = new List<GraduationCertificateDTO>();

        // Experiences
        public List<ExperienceDTO> Experiences { get; set; } = new List<ExperienceDTO>();

        public List<SpecialtiesDTO> Specialties { get; set; } = new List<SpecialtiesDTO>();
    }
}
