using Mohamy.Core.Entity.ApplicationData;
using Mohamy.Core.Entity.Files;
using Mohamy.Core.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mohamy.Core.Entity.ConsultingData
{
    public class Consulting : BaseEntity
    {
        [Required(ErrorMessage = "يجب إدخال استشارة فرعية")]
        [ForeignKey(nameof(subConsulting))]
        public string subConsultingId { get; set; }

        public subConsulting subConsulting { get; set; }

        [ForeignKey(nameof(Lawyer))]
        public string? LawyerId { get; set; }

        public ApplicationUser? Lawyer { get; set; }

        [Required(ErrorMessage = "يجب إدخال العميل")]
        [ForeignKey(nameof(Customer))]
        public string CustomerId { get; set; }

        public ApplicationUser Customer { get; set; }

        [Required(ErrorMessage = "يجب إدخال الوصف")]
        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "يجب أن تكون التكلفة رقمًا إيجابيًا")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceService { get; set; }

        public int OrderNumber { get; set; }

        public ICollection<Images>? Files { get; set; }

        public statusConsulting statusConsulting { get; set; } = statusConsulting.UserRequestedNotPaid;

        public ICollection<RequestConsulting>? RequestConsultings { get; set; }

        public int timeConsulting { get; set; }

        public DateTime? StartDate { get; set; } = DateTime.Now;

        public DateTime? EndDate { get; set; }
    }
}
