using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mohamy.Core.DTO.AuthViewModel.RegisterModel
{
    public class RegisterLawyer
    {
        [DisplayName("الاسم الكامل")]
        [Required(ErrorMessage = "يجب إدخال الاسم الكامل"), StringLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم الكامل 100 حرف")]
        public string FullName { get; set; }

        [DisplayName("رقم الهاتف")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "يجب إدخال رقم الهاتف"), StringLength(15, ErrorMessage = "يجب ألا يتجاوز رقم الهاتف 15 رقم")]
        public string PhoneNumber { get; set; }

        [DisplayName("كلمة المرور")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "يجب إدخال كلمة المرور")]
        public string Password { get; set; }

        [DisplayName("تأكيد كلمة المرور")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "يجب تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "كلمة المرور والتأكيد غير متطابقين")]
        public string ConfirmPassword { get; set; }

        [DisplayName("الصورة الشخصية")]
        public IFormFile? ImageProfile { get; set; }

        [DisplayName("شهاده التخرج")]
        public IFormFile? graduationCertificate { get; set; }

        [DisplayName("ترخيص المحاماه")]
        public IFormFile? lawyerLicense { get; set; }

        [DisplayName("الوصف")]
        [StringLength(500, ErrorMessage = "يجب ألا يتجاوز الوصف 500 حرف")]
        public string? Description { get; set; }

        [DisplayName("سنوات الخبرة")]
        [Range(0, 100, ErrorMessage = "يجب أن تكون سنوات الخبرة بين 0 و 100")]
        public int YearsExperience { get; set; }

        [DisplayName("المدينة")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز اسم المدينة 50 حرف")]
        public string? City { get; set; }

        [DisplayName("التخصص الأكاديمي")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز التخصص الأكاديمي 100 حرف")]
        public string? AcademicSpecialization { get; set; }

        [DisplayName("تكلفة الاستشارة")]
        public decimal CostConsulting { get; set; }

        [DisplayName("التعليم")]
        [StringLength(200, ErrorMessage = "يجب ألا يتجاوز التعليم 200 حرف")]
        public string? Education { get; set; }
    }
}
