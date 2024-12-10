using System.ComponentModel.DataAnnotations;

namespace Mohamy.Core.DTO.AuthViewModel.UpdateModel
{
    public class BankDetails
    {
        [Required]
        [Display(Name = "اسم البنك")]
        public string BankName { get; set; }

        [Required]
        [Display(Name = "رقم الحساب (12 رقم)")]
        [StringLength(maximumLength: 12, ErrorMessage = "رقم الحساب يجب أن يتكون من 12 رقم.",MinimumLength =12)]
        public string AccountNumber { get; set; }

        [Required]
        [Display(Name = "رقم الآيبان (14 رقم)")]
        [StringLength(14,ErrorMessage = "رقم الآيبان يجب أن يتكون من 14 رقم.",MinimumLength =14)]
        public string IBAN { get; set; }

        [Required]
        [Display(Name = "الاسم رباعي")]
        public string BeneficiaryName { get; set; }
    }
}
