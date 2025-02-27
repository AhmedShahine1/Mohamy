using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mohamy.Core.Entity.ConsultingData
{
    public class Transaction : BaseEntity
    {

        [Required(ErrorMessage = "يجب إدخال الاستشارة")]
        [ForeignKey(nameof(Consulting))]
        public string ConsultingId { get; set; }

        public Consulting Consulting { get; set; }


        [Required]
        public string PaymentId { get; set; }

        public decimal Amount { get; set; }
    }
}
