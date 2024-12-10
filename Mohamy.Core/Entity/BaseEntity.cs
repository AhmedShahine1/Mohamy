using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mohamy.Core.Entity
{
    public class BaseEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Display(Name = "تاريخ الإنشاء")]
        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public bool IsUpdated { get; set; } = false;

        [Display(Name = "تاريخ أخر تحديث  ")]
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; } = null;

        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;

        [Display(Name = "تاريخ الحذف  ")]
        [JsonIgnore]
        public DateTime? DeletedAt { get; set; } = null;
    }
}