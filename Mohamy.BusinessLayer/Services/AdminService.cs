using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.AuthViewModel;
using Mohamy.Core.DTO.AuthViewModel.LawyerDetailsModel;
using Mohamy.RepositoryLayer.Interfaces;

namespace Mohamy.BusinessLayer.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IFileHandling _fileHandling;

        public AdminService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountService = accountService;
            _fileHandling = fileHandling;
        }

        public async Task<IEnumerable<AuthDTO>> GetAllLawyersAsync()
        {
            var lawyerRoleId = await _unitOfWork.RoleRepository
                .FindAsync(r => r.Name == "Lawyer");

            var lawyers = from user in _unitOfWork.UserRepository.GetAll().AsQueryable()
                          .Include(a => a.Profile)
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

        public async Task<AuthDTO> GetLawyerByIdAsync(string lawyerId)
        {
            var lawyer = await _unitOfWork.UserRepository.FindAsync(
                q => q.Id == lawyerId,
                include: q => q.Include(a => a.Profile).Include(a => a.Documents)
            );

            if (lawyer == null)
                return null;

            var lawyerDTO = _mapper.Map<AuthDTO>(lawyer);

            // Fetch Lawyer License details
            var lawyerLicense = await _unitOfWork.lawyerLicenseRepository.FindAsync(q => q.LawyerId == lawyer.Id);
            lawyerDTO.lawyerLicenseId = lawyerLicense?.Id;
            lawyerDTO.lawyerLicenseStart = lawyerLicense?.Start;
            lawyerDTO.lawyerLicenseEnd = lawyerLicense?.End;
            lawyerDTO.lawyerLicenseNumber = lawyerLicense?.LicenseNumber;
            lawyerDTO.lawyerLicenseState = lawyerLicense?.State;

            // Fetch Graduation Certificates
            var lawyerGraduationCertificate = await _unitOfWork.graduationCertificateRepository.FindAllAsync(q => q.LawyerId == lawyer.Id);
            lawyerDTO.GraduationCertificates = _mapper.Map<List<GraduationCertificateDTO>>(lawyerGraduationCertificate);

            // Set Profile Image
            lawyerDTO.ProfileImage = await _accountService.GetUserProfileImage(lawyer.ProfileId);

            // Get Document URLs
            if (lawyer.Documents != null)
            {
                foreach (var doc in lawyer.Documents)
                {
                    var fileUrl = await _fileHandling.GetFile(doc.Id);
                    if (doc.path?.Name == "lawyerLicense")
                    {
                        lawyerDTO.lawyerLicenseURL.Add(fileUrl);
                    }
                    else if (doc.path?.Name == "graduationCertificate")
                    {
                        lawyerDTO.GraduationCertificatesURL.Add(fileUrl);
                    }
                }
            }

            // Handle "Not Found" cases
            if (!lawyerDTO.lawyerLicenseURL.Any())
            {
                lawyerDTO.lawyerLicenseURL.Add("NotFound");
            }
            if (!lawyerDTO.GraduationCertificatesURL.Any())
            {
                lawyerDTO.GraduationCertificatesURL.Add("NotFound");
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
