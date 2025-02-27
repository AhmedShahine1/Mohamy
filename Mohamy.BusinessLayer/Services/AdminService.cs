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
                          .Where(q=>!q.IsDeleted)
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
            if (string.IsNullOrEmpty(lawyerId))
            {
                Console.WriteLine("Lawyer ID is null or empty.");
                return null;
            }

            var lawyer = await _unitOfWork.UserRepository.FindAsync(
                q => q.Id == lawyerId,
                include: q => q.Include(a => a.Profile).Include(a => a.Documents)
            );

            if (lawyer == null)
            {
                Console.WriteLine($"Lawyer with ID {lawyerId} not found.");
                return null;
            }

            var lawyerDTO = _mapper.Map<AuthDTO>(lawyer);

            // Ensure Lists Are Initialized
            lawyerDTO.lawyerLicenseURL = new List<string>();
            lawyerDTO.GraduationCertificatesURL = new List<string>();

            // Lawyer License
            var lawyerLicense = await _unitOfWork.lawyerLicenseRepository.FindAsync(q => q.LawyerId == lawyer.Id);
            lawyerDTO.lawyerLicenseId = lawyerLicense?.Id;
            lawyerDTO.lawyerLicenseStart = lawyerLicense?.Start;
            lawyerDTO.lawyerLicenseEnd = lawyerLicense?.End;
            lawyerDTO.lawyerLicenseNumber = lawyerLicense?.LicenseNumber;
            lawyerDTO.lawyerLicenseState = lawyerLicense?.State;

            // Graduation Certificates
            var lawyerGraduationCertificate = await _unitOfWork.graduationCertificateRepository.FindAllAsync(q => q.LawyerId == lawyer.Id);
            lawyerDTO.GraduationCertificates = lawyerGraduationCertificate != null
                ? _mapper.Map<List<GraduationCertificateDTO>>(lawyerGraduationCertificate)
                : new List<GraduationCertificateDTO>();

            // Profile Image
            lawyerDTO.ProfileImage = lawyer.ProfileId != null ? await _accountService.GetUserProfileImage(lawyer.ProfileId) : "No Image";

            // Document URLs
            if (lawyer.Documents != null)
            {
                var pathLawyerLicense = await _unitOfWork.PathsRepository.FindAsync(q => q.Name == "lawyerLicense");
                var pathGraduationCertificate = await _unitOfWork.PathsRepository.FindAsync(q => q.Name == "graduationCertificate");

                foreach (var doc in lawyer.Documents)
                {
                    if (doc?.path != null)
                    {
                        var fileUrl = await _fileHandling.GetFile(doc.Id);

                        switch (doc.pathId)
                        {
                            case var pathId when pathId == pathLawyerLicense?.Id:
                                lawyerDTO.lawyerLicenseURL.Add(fileUrl);
                                break;

                            case var pathId when pathId == pathGraduationCertificate?.Id:
                                lawyerDTO.GraduationCertificatesURL.Add(fileUrl);
                                break;
                        }
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

        public async Task<int> GetCountLawyersAsync()
        {
            var lawyerRole = await _unitOfWork.RoleRepository
                .FindAsync(r => r.Name == "Lawyer");

            if (lawyerRole == null) return 0;

            var lawyerCount = _unitOfWork.UserRoleRepository.GetAll()
                .Count(ur => ur.RoleId == lawyerRole.Id);

            return lawyerCount;
        }

        public async Task<int> GetCountCustomersAsync()
        {
            var customerRole = await _unitOfWork.RoleRepository
                .FindAsync(r => r.Name == "Customer");

            if (customerRole == null) return 0;

            var customerCount = _unitOfWork.UserRoleRepository.GetAll()
                .Count(ur => ur.RoleId == customerRole.Id);

            return customerCount;
        }

        public async Task<int> GetCountAdminsAsync()
        {
            var adminRole = await _unitOfWork.RoleRepository
                .FindAsync(r => r.Name == "Admin");

            if (adminRole == null) return 0;

            var adminCount = _unitOfWork.UserRoleRepository.GetAll()
                .Count(ur => ur.RoleId == adminRole.Id);

            return adminCount;
        }

    }
}
