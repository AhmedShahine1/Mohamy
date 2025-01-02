using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Entity.ApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface ISubConsultingService : IService<SubConsultingDTO>
    {
        public Task<IEnumerable<ApplicationUser>> GetUsersBySubConsultingAsync(string subConsultingId);
        Task<List<SubConsultingDTO>> GetSubConsultingByMainAsync(string mainConsulting);
        Task<IEnumerable<SubConsultingDTO>> GetSubConsultingByUsersAsync(string UserId);
    }
}
