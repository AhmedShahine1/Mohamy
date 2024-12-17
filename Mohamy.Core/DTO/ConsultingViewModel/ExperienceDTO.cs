using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Mohamy.Core.DTO.ConsultingViewModel
{
    public class ExperienceDTO
    {
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string SubConsultingId { get; set; }
    }
}
