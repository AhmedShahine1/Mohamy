using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Mohamy.Core.DTO.AuthViewModel.UpdateModel
{
    public class UpdatePhone
    {
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "You should enter the Phone Number"), StringLength(15)]
        public string PhoneNumber { get; set; }
    }
}
