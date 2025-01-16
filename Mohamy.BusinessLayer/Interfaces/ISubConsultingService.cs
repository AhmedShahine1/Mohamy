using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Entity.ApplicationData;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface ISubConsultingService : IService<SubConsultingDTO>
    {
        public Task<IEnumerable<ApplicationUser>> GetUsersByMainConsultingAsync(string mainConsultingId);
        Task<List<SubConsultingDTO>> GetSubConsultingByMainAsync(string mainConsulting);
        Task<IEnumerable<MainConsultingDTO>> GetMainConsultingByUsersAsync(string UserId);
    }
}
