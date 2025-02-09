using Mohamy.Core.DTO.AuthViewModel.LawyerDetailsModel;
using Mohamy.Core.DTO.AuthViewModel.UpdateModel;
using Mohamy.Core.Entity.LawyerData;
using Mohamy.Core.Helpers;
using System.Collections.Generic;

namespace Mohamy.Core.DTO.AuthViewModel
{
    public class AuthDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string? Description { get; set; }
        public int? yearsExperience { get; set; }
        public string? City { get; set; }
        public string? Languages { get; set; }
        public string? AcademicQualification { get; set; }
        public bool Available { get; set; }
        public bool Online { get; set; }
        public string? academicSpecialization { get; set; }
        public string? Education { get; set; }
        public string ProfileImage { get; set; }
        public string ProfileImageId { get; set; }
        public LawyerRegistrationStatus RegistrationStatus { get; set; }
        public double? PriceService { get; set; }
        // Lawyer license details
        public string lawyerLicenseId { get; set; }
        public string lawyerLicenseNumber { get; set; }
        public string lawyerLicenseState { get; set; }
        public DateTime? lawyerLicenseStart { get; set; }
        public DateTime? lawyerLicenseEnd { get; set; }
        public List<string> lawyerLicenseURL { get; set; } = new List<string>();


        // Graduation certificates
        public List<GraduationCertificateDTO> GraduationCertificates { get; set; } = new List<GraduationCertificateDTO>();
        public List<string> GraduationCertificatesURL { get; set; } = new List<string>();

        // Specialties
        public List<SpecialtiesDTO> Specialties { get; set; } = new List<SpecialtiesDTO>();

        // Experiences
        public List<ExperienceDTO> Experiences { get; set; } = new List<ExperienceDTO>();

        public List<ProfessionDto> Professions { get; set; } = new List<ProfessionDto>();

        public int numberConsulting { get; set; }

        // Bank details
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? IBAN { get; set; }
        public bool Status { get; set; }
    }
}
