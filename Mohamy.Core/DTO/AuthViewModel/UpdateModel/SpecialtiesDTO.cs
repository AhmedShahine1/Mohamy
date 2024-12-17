using System.ComponentModel.DataAnnotations;

namespace Mohamy.Core.DTO.AuthViewModel.UpdateModel
{
    public class SpecialtiesDTO
    {
        [Required(ErrorMessage = "Sub-consulting ID is required.")]
        public string SubConsultingId { get; set; }
    }
}
