using Mohamy.Core.DTO.AuthViewModel;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<AuthDTO>> GetAllLawyersAsync();
        Task<IEnumerable<AuthDTO>> GetAllCustomersAsync();
        Task<IEnumerable<AuthDTO>> GetAllAdminsAsync(); // New method to get admins
        Task<AuthDTO> GetLawyerByIdAsync(string lawyerId);
        Task<int> GetCountLawyersAsync();
        Task<int> GetCountCustomersAsync();
        Task<int> GetCountAdminsAsync();
    }
}
