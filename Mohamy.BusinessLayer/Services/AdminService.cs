using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.AuthViewModel;
using Mohamy.RepositoryLayer.Interfaces;

namespace Mohamy.BusinessLayer.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public AdminService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<IEnumerable<AuthDTO>> GetAllLawyersAsync()
        {
            var lawyerRoleId = await _unitOfWork.RoleRepository
                .FindAsync(r => r.Name == "Lawyer");

            var lawyers = from user in _unitOfWork.UserRepository.GetAll().AsQueryable()
                          .Include(a => a.Profile)
                          .Include(a => a.lawyerLicense)
                          .Include(a => a.graduationCertificates)
                          .ToList()
                          join userRole in _unitOfWork.UserRoleRepository.GetAll().AsQueryable()
                          on user.Id equals userRole.UserId into userRoles
                          from ur in userRoles.DefaultIfEmpty()
                          where ur.RoleId == lawyerRoleId.Id
                          select user;

            var lawyerDTO = lawyers.Select(c => _mapper.Map<AuthDTO>(c)).ToList();
            foreach (var lawyer in lawyerDTO)
            {
                lawyer.ProfileImage = await _accountService.GetUserProfileImage(lawyer.ProfileImageId);
            }
            return lawyerDTO;
        }

        public async Task<IEnumerable<AuthDTO>> GetAllCustomersAsync()
        {
            var customerRoleId = await _unitOfWork.RoleRepository
                .FindAsync(r => r.Name == "Customer");

            var customers = from user in _unitOfWork.UserRepository.GetAll().AsQueryable()
                            join userRole in _unitOfWork.UserRoleRepository.GetAll().AsQueryable()
                            on user.Id equals userRole.UserId into userRoles
                            from ur in userRoles.DefaultIfEmpty()
                            where ur.RoleId == customerRoleId.Id
                            select user;

            var customersDTO = customers.Select(c => _mapper.Map<AuthDTO>(c)).ToList();
            foreach (var customer in customersDTO)
            {
                customer.ProfileImage = await _accountService.GetUserProfileImage(customer.ProfileImageId);
            }

            return customersDTO;
        }

        // New method to get all admins
        public async Task<IEnumerable<AuthDTO>> GetAllAdminsAsync()
        {
            var adminRoleId = await _unitOfWork.RoleRepository
                .FindAsync(r => r.Name == "Admin"); // Assuming "Admin" is the role name

            var admins = from user in _unitOfWork.UserRepository.GetAll().AsQueryable()
                         join userRole in _unitOfWork.UserRoleRepository.GetAll().AsQueryable()
                         on user.Id equals userRole.UserId into userRoles
                         from ur in userRoles.DefaultIfEmpty()
                         where ur.RoleId == adminRoleId.Id
                         select user;

            var adminDTO = admins.Select(c => _mapper.Map<AuthDTO>(c)).ToList();
            foreach (var admin in adminDTO)
            {
                admin.ProfileImage = await _accountService.GetUserProfileImage(admin.ProfileImageId);
            }

            return adminDTO;
        }
    }
}
