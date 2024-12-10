using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Helpers;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface IConsultingService
    {
        public Task<string> AddConsultingAsync(ConsultingDTO dto);
        public Task<IEnumerable<ConsultingDTO>> GetAllConsultingsAsync();
        public Task<IEnumerable<ConsultingDTO>> GetConsultingsByCustomerIdAsync(string customerId);
        public Task<IEnumerable<ConsultingDTO>> GetConsultingsByLawyerIdAsync(string lawyerId, statusConsulting status);
        public Task<ConsultingDTO> GetConsultingByIdAsync(string id);
        public Task UpdateConsultingStatusAsync(string id, statusConsulting status);
        public Task<IEnumerable<ConsultingDTO>> GetConsultingsWithoutLawyerAsync();
    }
}
