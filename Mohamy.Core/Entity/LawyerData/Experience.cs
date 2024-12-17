using Mohamy.Core.Entity.ApplicationData;
using Mohamy.Core.Entity.ConsultingData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mohamy.Core.Entity.LawyerData
{
    public class Experience
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "يجب إدخال الاستشارة الفرعية")]
        [ForeignKey(nameof(subConsulting))]
        public string subConsultingId { get; set; }
        public subConsulting subConsulting { get; set; }


        [Required(ErrorMessage = "يجب إدخال المحامي")]
        [ForeignKey(nameof(Lawyer))]
        public string LawyerId { get; set; }
        public ApplicationUser Lawyer { get; set; }
    }
}
