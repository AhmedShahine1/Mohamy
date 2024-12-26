using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Helpers;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface IConsultingService
    {
        public Task<string> AddConsultingAsync(ConsultingDTO dto);
        public Task<IEnumerable<ConsultingDTO>> GetAllConsultingsAsync();
        public Task<IEnumerable<ConsultingDTO>> GetConsultingsByCustomerIdAsync(string customerId);
        Task<IEnumerable<ConsultingDTO>> GetConsultingsInprogress(string customerId);
        Task<IEnumerable<ConsultingDTO>> GetConsultingsCompleted(string customerId);
        Task<IEnumerable<ConsultingDTO>> GetConsultingsCancelled(string customerId);
        Task<IEnumerable<ConsultingDTO>> GetServicesInprogress(string customerId);
        Task<IEnumerable<ConsultingDTO>> GetServicesCompleted(string customerId);
        Task<IEnumerable<ConsultingDTO>> GetServicesCancelled(string customerId);
        Task<IEnumerable<ConsultingDTO>> GetServices(string customerId);
        public Task<IEnumerable<ConsultingDTO>> GetConsultingsByLawyerIdAsync(string lawyerId, statusConsulting status);
        public Task<ConsultingDTO> GetConsultingByIdAsync(string id);
        public Task UpdateConsultingStatusAsync(string id, statusConsulting status);
        public Task<IEnumerable<ConsultingDTO>> GetAvailableConsultations();
    }
}
