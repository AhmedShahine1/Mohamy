using Mohamy.Core.Entity.ApplicationData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mohamy.Core.Entity.ConsultingData
{
    public class IgnoredConsultation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [ForeignKey(nameof(Lawyer))]
        public string LawyerId { get; set; }
        public ApplicationUser Lawyer { get; set; }

        [Required]
        [ForeignKey(nameof(Consulting))]
        public string consultingId { get; set; }

        public Consulting consulting { get; set; }
    }
}
