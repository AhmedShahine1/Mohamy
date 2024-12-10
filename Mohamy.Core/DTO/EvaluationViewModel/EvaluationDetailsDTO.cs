using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.Core.DTO.EvaluationViewModel
{
    public class EvaluationDetailsDTO
    {
        public string EvaluatorId { get; set; }
        public string EvaluatorName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
    }
}