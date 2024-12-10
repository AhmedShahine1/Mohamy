using Mohamy.Core.Entity.ApplicationData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.Core.Entity.Others
{
    public class Evaluation :BaseEntity
    {
        [Required]
        [ForeignKey(nameof(Evaluator))]
        public string EvaluatorId { get; set; }
        public ApplicationUser Evaluator { get; set; }

        [Required]
        [ForeignKey(nameof(Evaluated))]
        public string EvaluatedId { get; set; }
        public ApplicationUser Evaluated { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }
    }
}
