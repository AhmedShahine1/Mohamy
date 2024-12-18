using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.Core.DTO.AuthViewModel.LawyerDetailsModel
{
    public class ExperienceDTO
    {
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string subConsultingName { get; set; } // Name of the sub-consulting associated with the experience.
        public string SubConsultingId { get; set; }
    }
}
