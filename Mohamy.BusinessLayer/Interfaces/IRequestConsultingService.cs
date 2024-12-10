using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface IRequestConsultingService
    {
        public Task<RequestConsultingDTO> AddRequestAsync(RequestConsultingDTO requestConsultingDTO);
        public Task<bool> UpdateRequestStatusAsync(string requestId, statusRequestConsulting newStatus);
        public Task<IEnumerable<RequestConsultingDTO>> GetRequestsByUserAsync(string userId);
        public Task<IEnumerable<RequestConsultingDTO>> GetRequestsByConsultingAsync(string consultingId);
        public Task<IEnumerable<RequestConsultingDTO>> GetAllRequestsAsync();

    }
}
