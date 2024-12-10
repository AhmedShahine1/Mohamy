using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.Core.DTO.ConsultingViewModel
{
    public class MainConsultingDTO
    {
        public string? Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public string? IconUrl { get; set; }
        public IFormFile Icon { get; set; }
        public ICollection<SubConsultingDTO>? SubConsultings { get; set; }
        public bool service { get; set; }
    }
}
