using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Mohamy.Core.DTO.AuthViewModel.UpdateModel
{
    public class UpdatePrice
    {
        [DisplayName("PriceService")]
        [Required(ErrorMessage = "You should enter Price")]
        public double PriceService { get; set; }
    }
}
