using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Mohamy.Core.DTO.AuthViewModel.UpdateModel
{
    public class QualificationUser
    {
        [DisplayName("الوصف")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [DisplayName("سنين الخبره")]
        [Range(0, 100, ErrorMessage = "Years of experience must be between 0 and 100")]
        public int YearsExperience { get; set; }

        [DisplayName("المدينه")]
        [StringLength(50, ErrorMessage = "City name cannot exceed 50 characters")]
        public string City { get; set; }

        [DisplayName("التخصص الاكاديمي")]
        [StringLength(100, ErrorMessage = "Academic Specialization cannot exceed 100 characters")]
        public string AcademicSpecialization { get; set; }

        [DisplayName("تكلفه الاستشارة")]
        public decimal CostConsulting { get; set; }

        [DisplayName("التعليم")]
        [StringLength(200, ErrorMessage = "Education details cannot exceed 200 characters")]
        public string Education { get; set; }

        [DisplayName("ترخيص المحاماه")]
        public IFormFile? lawyerLicense { get; set; }

        [DisplayName("شهاده التخرج")]
        public IFormFile? graduationCertificate { get; set; }


    }
}
