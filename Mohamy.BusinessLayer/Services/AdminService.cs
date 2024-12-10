//using AutoMapper;
//using Microsoft.EntityFrameworkCore;
//using Mohamy.BusinessLayer.Interfaces;
//using Mohamy.Core.DTO.AuthViewModel;
//using Mohamy.Core.DTO.ConsultingViewModel;
//using Mohamy.Core.Entity.ApplicationData;
//using Mohamy.Core.Entity.ConsultingData;
//using Mohamy.RepositoryLayer.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Mohamy.BusinessLayer.Services
//{
//    public class AdminService : IAdminService
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;
//        private readonly IAccountService _accountService;
//        private readonly IExperienceService _experienceService;

//        public AdminService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService, IExperienceService experienceService)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//            _accountService = accountService;
//            _experienceService = experienceService;
//        }

//        // Method to get all lawyers
//        public async Task<IEnumerable<AuthDTO>> GetAllLawyersAsync()
//        {
//            var lawyerRoleId = await _unitOfWork.RoleRepository
//                .FindAsync(r => r.Name == "Lawyer"); // Assuming "Lawyer" is the role name

//            var lawyers = from user in _unitOfWork.UserRepository.GetAll().AsQueryable().Include(a => a.Profile)
//                          //.Include(a => a.lawyerLicense)
//                          //.Include(a => a.graduationCertificate)
//                          join userRole in _unitOfWork.UserRoleRepository.GetAll().AsQueryable()
//                          on user.Id equals userRole.UserId into userRoles
//                          from ur in userRoles.DefaultIfEmpty()
//                          where ur.RoleId == lawyerRoleId.Id
//                          select user;

//            var lawyerDTO = lawyers.Select(c => _mapper.Map<AuthDTO>(c)).ToList();
//            foreach (var lawyer in lawyerDTO)
//            {
//                lawyer.ProfileImage = await _accountService.GetUserProfileImage(lawyer.ProfileImageId);
//                lawyer.lawyerLicense = await _accountService.GetUserProfileImage(lawyer.lawyerLicenseId);
//                lawyer.graduationCertificate = await _accountService.GetUserProfileImage(lawyer.graduationCertificateId);
//                var experiences = await _experienceService.GetAllExperiencesByUserIdAsync(lawyer.Id);
//                lawyer.ExperienceNames = experiences.Select(e => e.subConsulting.Name).ToList();
//            }
//            return lawyerDTO;
//        }

//            // Method to get all customers
//            public async Task<IEnumerable<AuthDTO>> GetAllCustomersAsync()
//        {
//            var customerRoleId = await _unitOfWork.RoleRepository
//                .FindAsync(r => r.Name == "Customer"); // Assuming "Customer" is the role name

//            var customers = from user in _unitOfWork.UserRepository.GetAll().AsQueryable()
//                            join userRole in _unitOfWork.UserRoleRepository.GetAll().AsQueryable()
//                            on user.Id equals userRole.UserId into userRoles
//                            from ur in userRoles.DefaultIfEmpty()
//                            where ur.RoleId == customerRoleId.Id
//                            select user;
//            var customersDTO = customers.Select(c => _mapper.Map<AuthDTO>(c)).ToList();
//            foreach (var customer in customersDTO)
//            {
//                customer.ProfileImage = await _accountService.GetUserProfileImage(customer.ProfileImageId);
//            }

//            return customersDTO;
//        }
//    }
//}
