using System.Collections.Generic;

namespace Mohamy.Core.DTO.AuthViewModel
{
    public class AuthDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public string? Description { get; set; }
        public int? yearsExperience { get; set; }
        public string? City { get; set; }
        public string? academicSpecialization { get; set; }
        public string? Education { get; set; }
        public string ProfileImage { get; set; }
        public string ProfileImageId { get; set; }
        public string lawyerLicense { get; set; }
        public string lawyerLicenseId { get; set; }
        public string graduationCertificate { get; set; }
        public string graduationCertificateId { get; set; }
        public int numberConsulting { get; set; }
        public decimal CostConsulting30 { get; set; }
        public decimal CostConsulting60 { get; set; }
        public decimal CostConsulting90 { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? IBAN { get; set; }
        public List<string> ExperienceNames { get; set; } // List of experience names, mapped from the Experience collection in ApplicationUser.
    }
}
