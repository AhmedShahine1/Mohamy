using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.Core.DTO.AuthViewModel.LawyerDetailsModel
{
    public class GraduationCertificateDTO
    {
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Country { get; set; }
        public string Collage { get; set; }
        public string University { get; set; }
        public string Description { get; set; }
    }
}
