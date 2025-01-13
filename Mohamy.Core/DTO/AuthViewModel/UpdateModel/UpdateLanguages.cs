using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Mohamy.Core.DTO.AuthViewModel.UpdateModel
{
    public class UpdateLanguages
    {
        [DisplayName("Languages")]
        [Required(ErrorMessage = "You should enter Languages")]
        public string Languages { get; set; }
    }
}
