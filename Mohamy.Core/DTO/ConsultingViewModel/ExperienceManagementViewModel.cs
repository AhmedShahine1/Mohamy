using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.Core.DTO.ConsultingViewModel
{
    public class ExperienceManagementViewModel
    {
        public string LaywerId { get; set; }
        public List<ExperienceDTO> Experiences { get; set; }
        public IEnumerable<SelectListItem> SubConsultings { get; set; }
    }
}
