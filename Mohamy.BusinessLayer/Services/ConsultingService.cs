using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.AuthViewModel;
using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Entity.ConsultingData;
using Mohamy.Core.Entity.Files;
using Mohamy.Core.Helpers;
using Mohamy.RepositoryLayer.Interfaces;

namespace Mohamy.BusinessLayer.Services
{
    public class ConsultingService : IConsultingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IFileHandling _fileHandling;

        public ConsultingService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountService = accountService;
            _fileHandling = fileHandling;
        }

        public async Task<int> GetNextOrderNumberAsync()
        {
            var maxOrderNumber = _unitOfWork.ConsultingRepository.GetAll().OrderByDescending(c => c.OrderNumber)
            .Select(c => c.OrderNumber)
            .FirstOrDefault();

            return maxOrderNumber + 1; // Increment the maximum value
        }

        public async Task<string> AddConsultingAsync(ConsultingDTO dto)
        {
            var consulting = await CreateConsultingAsync(dto);
            consulting.OrderNumber = GetNextOrderNumberAsync().Result;
            await _unitOfWork.ConsultingRepository.AddAsync(consulting);
            await _unitOfWork.SaveChangesAsync();
            return consulting.Id;
        }

        private async Task<Consulting> CreateConsultingAsync(ConsultingDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var consulting = new Consulting
            {
                Id = dto.Id,
                subConsultingId = dto.SubConsultingId,
                LawyerId = dto.LaywerId,
                Lawyer = dto.LaywerId != null ? await _accountService.GetUserById(dto.LaywerId) : null,
                CustomerId = dto.CustomerId,
                Customer = await _accountService.GetUserById(dto.CustomerId),
                Description = dto.Description,
                statusConsulting = Enum.TryParse<statusConsulting>(dto.StatusConsulting, out var status) ? status : statusConsulting.UserRequestedNotPaid
            };
            if(dto.LaywerId != null)
            {
                if (dto.timeConsulting == 30)
                {
                    consulting.PriceService = dto.Lawyer.CostConsulting30;
                    consulting.StartDate = DateTime.UtcNow;
                    consulting.EndDate = DateTime.UtcNow.AddMinutes(30);
                }
                else if( dto.timeConsulting == 60)
                {
                    consulting.PriceService = dto.Lawyer.CostConsulting60;
                    consulting.StartDate = DateTime.UtcNow;
                    consulting.EndDate = DateTime.UtcNow.AddMinutes(60);
                }
                else
                {
                    consulting.PriceService = dto.Lawyer.CostConsulting90;
                    consulting.StartDate = DateTime.UtcNow;
                    consulting.EndDate = DateTime.UtcNow.AddMinutes(90);
                }
            }
            if (dto.Files != null && dto.Files.Any())
            {
                var path = await _accountService.GetPathByName("ConsultingFiles");
                consulting.Files = new List<Images>();

                foreach (var file in dto.Files)
                {
                    string fileId = await _fileHandling.UploadFile(file, path);
                    consulting.Files.Add(await _unitOfWork.ImagesRepository.FindAsync(a => a.Id == fileId));
                }
            }

            return consulting;
        }

        public async Task<IEnumerable<ConsultingDTO>> GetAllConsultingsAsync()
        {
            var consultings = await _unitOfWork.ConsultingRepository.GetAllAsync(include: q => q
                .Include(c => c.subConsulting)
                .Include(c => c.Lawyer)
                .Include(c => c.Customer)
                .Include(c => c.Files));

            var consultingDTOs = _mapper.Map<IEnumerable<ConsultingDTO>>(consultings.Where(c => c.statusConsulting != statusConsulting.Cancelled));
            // After completing all database operations, fetch profile images asynchronously
            foreach (var consulting in consultingDTOs)
            {
                if (consulting.Lawyer != null && !string.IsNullOrEmpty(consulting.Lawyer.ProfileImageId))
                {
                    consulting.Lawyer.ProfileImage = await _accountService.GetUserProfileImage(consulting.Lawyer.ProfileImageId);
                }

                if (consulting.Customer != null && !string.IsNullOrEmpty(consulting.Customer.ProfileImageId))
                {
                    consulting.Customer.ProfileImage = await _accountService.GetUserProfileImage(consulting.Customer.ProfileImageId);
                }
                if (consulting.RequestConsultings != null)
                {
                    consulting.RequestConsultings = _mapper.Map<List<RequestConsultingDTO>>(_unitOfWork.RequestConsultingRepository.FindAllAsync(a => a.ConsultingId == consulting.Id).Result);
                }
                consulting.SubConsultingName = _unitOfWork.SubConsultingRepository.GetByIdAsync(consulting.SubConsultingId).Result.Name;
                consulting.FilesUrl = new List<string>();
                foreach (var consulting1 in consultings)
                {
                    if (consulting1.Id == consulting.Id)
                    {
                        foreach (var file in consulting1.Files)
                        {
                            consulting.FilesUrl.Add(await _fileHandling.GetFile(file.Id));
                        }
                    }
                }
            }

            return consultingDTOs.ToList();
        }

        public async Task<IEnumerable<ConsultingDTO>> GetConsultingsByCustomerIdAsync(string customerId)
        {
            // Retrieve consultings and related data from the database first
            var consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                a => a.CustomerId == customerId && a.statusConsulting != statusConsulting.Cancelled,
                include: q => q
                    .Include(c => c.subConsulting)
                    .Include(c => c.Lawyer)
                    .Include(c => c.Customer)
                    .Include(c => c.Files)
            );

            // Map consulting entities to DTOs
            var consultingDTOs = _mapper.Map<List<ConsultingDTO>>(consultings.Where(c => !c.IsDeleted));

            // After completing all database operations, fetch profile images asynchronously
            foreach (var consulting in consultingDTOs)
            {
                if (consulting.Lawyer != null && !string.IsNullOrEmpty(consulting.Lawyer.ProfileImageId))
                {
                    consulting.Lawyer.ProfileImage = await _accountService.GetUserProfileImage(consulting.Lawyer.ProfileImageId);
                }

                if (consulting.Customer != null && !string.IsNullOrEmpty(consulting.Customer.ProfileImageId))
                {
                    consulting.Customer.ProfileImage = await _accountService.GetUserProfileImage(consulting.Customer.ProfileImageId);
                }
                if (consulting.RequestConsultings != null)
                {
                    consulting.RequestConsultings = _mapper.Map<List<RequestConsultingDTO>>(_unitOfWork.RequestConsultingRepository.FindAllAsync(a => a.ConsultingId == consulting.Id).Result);
                }
                consulting.SubConsultingName = _unitOfWork.SubConsultingRepository.GetByIdAsync(consulting.SubConsultingId).Result.Name;
                consulting.FilesUrl = new List<string>();
                foreach (var consulting1 in consultings)
                {
                    if (consulting1.Id == consulting.Id)
                    {
                        foreach (var file in consulting1.Files)
                        {
                            consulting.FilesUrl.Add(await _fileHandling.GetFile(file.Id));
                        }
                    }
                }
            }

            return consultingDTOs;
        }

        public async Task<IEnumerable<ConsultingDTO>> GetConsultingsByLawyerIdAsync(string lawyerId, statusConsulting status)
        {
            var consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                a => a.statusConsulting == status && a.statusConsulting != statusConsulting.Cancelled,
                include: q => q
                    .Include(c => c.subConsulting)
                    .Include(c => c.Lawyer)
                    .Include(c => c.Customer)
                    .Include(c => c.Files)
            );

            var consultingDTOs = _mapper.Map<IEnumerable<ConsultingDTO>>(consultings.Where(c => !c.IsDeleted));

            // After completing all database operations, fetch profile images asynchronously
            foreach (var consulting in consultingDTOs)
            {
                if (consulting.Lawyer != null && !string.IsNullOrEmpty(consulting.Lawyer.ProfileImageId))
                {
                    consulting.Lawyer.ProfileImage = await _accountService.GetUserProfileImage(consulting.Lawyer.ProfileImageId);
                }

                if (consulting.Customer != null && !string.IsNullOrEmpty(consulting.Customer.ProfileImageId))
                {
                    consulting.Customer.ProfileImage = await _accountService.GetUserProfileImage(consulting.Customer.ProfileImageId);
                }
                if (consulting.RequestConsultings != null)
                {
                    consulting.RequestConsultings = _mapper.Map<List<RequestConsultingDTO>>(_unitOfWork.RequestConsultingRepository.FindAllAsync(a => a.ConsultingId == consulting.Id).Result);
                }
                consulting.SubConsultingName = _unitOfWork.SubConsultingRepository.GetByIdAsync(consulting.SubConsultingId).Result.Name;
                consulting.FilesUrl = new List<string>();
                foreach (var consulting1 in consultings)
                {
                    if (consulting1.Id == consulting.Id)
                    {
                        foreach (var file in consulting1.Files)
                        {
                            consulting.FilesUrl.Add(await _fileHandling.GetFile(file.Id));
                        }
                    }
                }
            }

            return consultingDTOs.ToList();
        }

        public async Task<ConsultingDTO> GetConsultingByIdAsync(string id)
        {
            var consulting = await _unitOfWork.ConsultingRepository.FindAsync(
                c => c.Id == id && c.statusConsulting != statusConsulting.Cancelled,
                include: q => q
                    .Include(c => c.subConsulting)
                    .Include(c => c.Lawyer)
                    .Include(c => c.Customer)
                    .Include(c => c.Files)
            );

            if (consulting == null)
                return null;

            var consultingDTO = _mapper.Map<ConsultingDTO>(consulting);

            // Retrieve profile images for Lawyer and Customer
            if (consultingDTO.Lawyer != null && !string.IsNullOrEmpty(consultingDTO.Lawyer.ProfileImageId))
            {
                consultingDTO.Lawyer.ProfileImage = await _accountService.GetUserProfileImage(consultingDTO.Lawyer.ProfileImageId);
            }
            var requestconsulting = await _unitOfWork.RequestConsultingRepository.FindAsync(a => a.ConsultingId == id && a.statusRequestConsulting == statusRequestConsulting.Waiting);
            consultingDTO.RequestConsultings = _mapper.Map<ICollection<RequestConsultingDTO>>(requestconsulting);
            foreach (var request in consultingDTO.RequestConsultings)
            {
                request.Lawyer = _mapper.Map<AuthDTO>(await _unitOfWork.UserRepository.FindAsync(a => a.Id == request.LawyerId));
                if (request.Lawyer != null && !string.IsNullOrEmpty(request.Lawyer.ProfileImageId))
                {
                    request.Lawyer.ProfileImage = await _accountService.GetUserProfileImage(request.Lawyer.ProfileImageId);
                }
            }
            if (consultingDTO.Customer != null && !string.IsNullOrEmpty(consultingDTO.Customer.ProfileImageId))
            {
                consultingDTO.Customer.ProfileImage = await _accountService.GetUserProfileImage(consultingDTO.Customer.ProfileImageId);
            }
            consultingDTO.FilesUrl = new List<string>();
            foreach (var file in consulting.Files)
            {
                consultingDTO.FilesUrl.Add(await _fileHandling.GetFile(file.Id));
            }
            return consultingDTO;
        }

        public async Task UpdateConsultingStatusAsync(string id, statusConsulting status)
        {
            var consulting = await _unitOfWork.ConsultingRepository.GetByIdAsync(id);
            if (consulting == null) throw new ArgumentException("Consulting not found");
            if (consulting.LawyerId == null && status != statusConsulting.Cancelled) throw new ArgumentException("Consulting should Choose Lawyer");
            if (statusConsulting.InProgress == status) consulting.EndDate = DateTime.Now.AddDays(2);
            consulting.statusConsulting = status;
            consulting.IsUpdated = true;
            consulting.UpdatedAt = DateTime.Now;

            _unitOfWork.ConsultingRepository.Update(consulting);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ConsultingDTO>> GetConsultingsWithoutLawyerAsync()
        {
            var consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                c => c.LawyerId == null && !c.IsDeleted,
                include: q => q
                    .Include(c => c.subConsulting)
                    .Include(c => c.Customer)
                    .Include(c => c.Files)
            );

            var consultingDTOs = _mapper.Map<IEnumerable<ConsultingDTO>>(consultings.Where(c => c.statusConsulting != statusConsulting.Cancelled));

            // After completing all database operations, fetch profile images asynchronously
            foreach (var consulting in consultingDTOs)
            {
                if (consulting.Lawyer != null && !string.IsNullOrEmpty(consulting.Lawyer.ProfileImageId))
                {
                    consulting.Lawyer.ProfileImage = await _accountService.GetUserProfileImage(consulting.Lawyer.ProfileImageId);
                }

                if (consulting.Customer != null && !string.IsNullOrEmpty(consulting.Customer.ProfileImageId))
                {
                    consulting.Customer.ProfileImage = await _accountService.GetUserProfileImage(consulting.Customer.ProfileImageId);
                }
            }


            return consultingDTOs.ToList();
        }

    }

}
