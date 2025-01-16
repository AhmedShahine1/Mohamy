using System.ComponentModel.DataAnnotations;

namespace Mohamy.Core.DTO.NotificationViewModel
{
    public class SaveDeviceDTO
    {
        [Required]
        public string DeviceId { get; set; }
    }
}
