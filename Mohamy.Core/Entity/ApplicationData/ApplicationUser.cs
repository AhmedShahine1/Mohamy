using Mohamy.Core.Entity.Files;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Mohamy.Core.Entity.LawyerData;
using Mohamy.Core.Entity.ConsultingData;
using Mohamy.Core.Entity.Others;
using Mohamy.Core.Helpers;

namespace Mohamy.Core.Entity.ApplicationData
{
    [DebuggerDisplay("{FullName,nq}")]
    public class ApplicationUser : IdentityUser
    {
        public bool Status { get; set; } = true; // يدل على ما إذا كان الحساب نشطًا أم لا.

        public string FullName { get; set; }
        public override string? Email { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now; // يتم ضبط تاريخ التسجيل تلقائيًا.

        public string? Description { get; set; }

        public int? yearsExperience { get; set; }

        public string? City { get; set; }

        public string? Region { get; set; }

        public string? BankName { get; set; }

        public string? AccountNumber { get; set; }

        public string? BeneficiaryName { get; set; }

        public string? IBAN { get; set; }
        public double? PriceService { get; set; }
        public string? AcademicQualification { get; set; }
        public string? Languages { get; set; }
        public bool Available { get; set; } = false;
        public bool Online { get; set; } = false;

        [ForeignKey(nameof(Profile))]
        public string ProfileId { get; set; }

        public Images Profile { get; set; } // صورة الملف الشخصي للمستخدم.

        public LawyerRegistrationStatus RegistrationStatus { get; set; } = LawyerRegistrationStatus.NotLawyer;

        public IEnumerable<graduationCertificate> graduationCertificates { get; set; } = new List<graduationCertificate>();

        [ForeignKey(nameof(lawyerLicense))]
        public string? lawyerLicenseId { get; set; }
        public lawyerLicense? lawyerLicense { get; set; }

        public ICollection<Profession>? Professions { get; set; }
        public ICollection<Experience>? Experiences { get; set; }

        public ICollection<Specialties>? Specialties { get; set; }

        public bool professionalAccreditation { get; set; } = false;

        public ICollection<RequestConsulting>? RequestConsultings { get; set; }

        public ICollection<Evaluation> EvaluationsReceived { get; set; } = new List<Evaluation>();

        public ICollection<Images>? Documents { get; set; }

    }
}
