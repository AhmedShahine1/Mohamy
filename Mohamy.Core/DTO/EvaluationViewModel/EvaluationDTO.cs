using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.Core.DTO.EvaluationViewModel
{
    public class EvaluationDTO
    {
        public string EvaluatedId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
