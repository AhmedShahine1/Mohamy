using Mohamy.Core.Entity.ApplicationData;
using Mohamy.Core.Entity.ConsultingData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.Core.Entity.LawyerData
{
    public class Specialties
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [ForeignKey(nameof(Lawyer))]
        public string LawyerId { get; set; }
        public ApplicationUser Lawyer { get; set; }

        [Required(ErrorMessage = "يجب إدخال الاستشارة الفرعية")]
        [ForeignKey(nameof(subConsulting))]
        public string subConsultingId { get; set; }

        public subConsulting subConsulting { get; set; }
    }
}
