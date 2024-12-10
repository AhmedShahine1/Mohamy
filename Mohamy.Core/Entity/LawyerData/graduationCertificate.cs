using Mohamy.Core.Entity.ConsultingData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mohamy.Core.Entity.ApplicationData;

namespace Mohamy.Core.Entity.LawyerData
{
    public class graduationCertificate
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string Collage { get; set; }
        public string University { get; set; }
        [ForeignKey(nameof(Lawyer))]
        public string LawyerId { get; set; }
        public ApplicationUser Lawyer { get; set; }
    }
}
