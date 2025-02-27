using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Entity.ApplicationData;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface ISubConsultingService : IService<SubConsultingDTO>
    {
        public Task<IEnumerable<ApplicationUser>> GetUsersBySubConsultingAsync(string subConsultingId);
        Task<List<SubConsultingDTO>> GetSubConsultingByMainAsync(string mainConsulting);
        Task<IEnumerable<SubConsultingDTO>> GetSubConsultingByUsersAsync(string UserId);
        Task<List<SpecialityDTO>> GetAllSpecialitiesAsync();
    }
}
