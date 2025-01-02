using Mohamy.Core.Entity.Files;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mohamy.Core.Entity.ConsultingData
{
    public class subConsulting : BaseEntity
    {
        [Required(ErrorMessage = "يجب إدخال الاسم")]
        [StringLength(250, ErrorMessage = "الاسم لا يمكن أن يتجاوز 250 حرفًا")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يجب إدخال الوصف")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "الوصف يجب أن يكون بين 5 و 1000 حرف")]
        public string Description { get; set; }

        [Required(ErrorMessage = "يجب إدخال أيقونة")]
        [ForeignKey(nameof(Icon))]
        public string iconId { get; set; }

        public Images Icon { get; set; }

        [Required(ErrorMessage = "يجب إدخال الاستشارة الرئيسية")]
        [ForeignKey(nameof(MainConsulting))]
        public string MainConsultingId { get; set; }

        public mainConsulting MainConsulting { get; set; }

        public bool NotKnow { get; set; } = false;
    }
}
