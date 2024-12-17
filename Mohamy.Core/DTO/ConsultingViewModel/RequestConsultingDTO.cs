using Mohamy.Core.DTO.AuthViewModel;
using Mohamy.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.Core.DTO.ConsultingViewModel
{
    public class RequestConsultingDTO
    {
        public string? Id { get; set; } = Guid.NewGuid().ToString();
        public string? LawyerId { get; set; }
        public AuthDTO? Lawyer { get; set; }
        public string ConsultingId { get; set; }
        public ConsultingDTO? Consulting { get; set; }
        public decimal PriceService { get; set; }
        public statusRequestConsulting StatusRequestConsultingEnum { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public string StatusRequestConsulting => StatusRequestConsultingHelper.GetArabicTranslation(StatusRequestConsultingEnum);

    }
}
