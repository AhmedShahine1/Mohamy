using Mohamy.Core.Entity.ApplicationData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.Core.Entity.LawyerData
{
    public class lawyerLicense
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string LicenseNumber { get; set; }
        public DateTime Start {  get; set; }
        public DateTime End { get; set; }
        public string State { get; set; }
        [ForeignKey(nameof(Lawyer))]
        public string LawyerId { get; set; }
        public ApplicationUser Lawyer { get; set; }
    }
}
