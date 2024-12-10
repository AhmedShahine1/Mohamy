using Mohamy.Core.Entity.ApplicationData;
using Mohamy.Core.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mohamy.Core.Entity.ConsultingData
{
    public class RequestConsulting : BaseEntity
    {
        [Required(ErrorMessage = "يجب إدخال المحامي")]
        [ForeignKey(nameof(Lawyer))]
        public string LawyerId { get; set; }

        public ApplicationUser Lawyer { get; set; }

        [Required(ErrorMessage = "يجب إدخال الاستشارة")]
        [ForeignKey(nameof(Consulting))]
        public string ConsultingId { get; set; }

        public Consulting Consulting { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "يجب أن تكون التكلفة رقمًا إيجابيًا")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceService { get; set; }

        public statusRequestConsulting statusRequestConsulting { get; set; } = statusRequestConsulting.Waiting;
    }
}
