using Mohamy.Core.Entity.ApplicationData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mohamy.Core.Entity.LawyerData
{
    public class Profession
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "يجب إدخال المحامي")]
        [ForeignKey(nameof(Lawyer))]
        public string LawyerId { get; set; }
        public ApplicationUser Lawyer { get; set; }
    }
}
