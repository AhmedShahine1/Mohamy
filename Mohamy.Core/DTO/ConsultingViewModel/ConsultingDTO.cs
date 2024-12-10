using Microsoft.AspNetCore.Http;
using Mohamy.Core.DTO.AuthViewModel;
using Mohamy.Core.Helpers;

namespace Mohamy.Core.DTO.ConsultingViewModel
{
    public class ConsultingDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int OrderNumber { get; set; }
        public string SubConsultingId { get; set; }
        public string? LaywerId { get; set; }
        public string? CustomerId { get; set; }
        public AuthDTO? Lawyer { get; set; }
        public AuthDTO? Customer { get; set; }
        public string Description { get; set; }
        public decimal PriceService { get; set; }
        public string? SubConsultingName { get; set; }
        public int timeConsulting { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public ICollection<RequestConsultingDTO>? RequestConsultings { get; set; }
        public ICollection<IFormFile>? Files { get; set; }
        public ICollection<string>? FilesUrl { get; set; }
        public statusConsulting StatusConsultingEnum { get; set; }

        // This property will return the Arabic translation of the status
        public string StatusConsulting => StatusConsultingHelper.GetArabicTranslation(StatusConsultingEnum);
    }
}
