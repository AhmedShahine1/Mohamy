using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Mohamy.Core.DTO.ConsultingViewModel
{
    public class ExperienceDTO
    {
        public string? Id { get; set; }
        public string? LaywerId { get; set; }

        // List of selected subConsulting IDs
        public List<string>? subConsultingIds { get; set; } = new List<string>();

        // List of available subConsultings for selection
        public IEnumerable<SelectListItem>? SubConsultings { get; set; }
    }
}
