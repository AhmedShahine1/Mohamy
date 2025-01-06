using System.ComponentModel.DataAnnotations;

namespace Mohamy.Core.DTO.AuthViewModel.UpdateModel
{
    public class UpdateBank
    {
        [Required(ErrorMessage = "You should enter Bank Name")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "You should enter Account Number")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "You should enter BeneficiaryName")]
        public string BeneficiaryName { get; set; }
    }
}
