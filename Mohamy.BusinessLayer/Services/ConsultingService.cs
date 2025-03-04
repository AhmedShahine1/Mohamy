﻿using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.DTO.NotificationViewModel;
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
        private readonly INotificationService _notificationService;

        public ConsultingService(IUnitOfWork unitOfWork, INotificationService notificationService, IMapper mapper, IAccountService accountService, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountService = accountService;
            _fileHandling = fileHandling;
            _notificationService = notificationService;
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
                Title = dto.Title,
                voiceConsulting = dto.voiceConsulting,
                statusConsulting = Enum.TryParse<statusConsulting>(dto.StatusConsulting, out var status) ? status : statusConsulting.UserRequestedNotPaid
            };
            if (dto.LaywerId != null)
            {
                consulting.PriceService = (decimal)((consulting.voiceConsulting) ? 100 : consulting.Lawyer.PriceService);
                consulting.StartDate = DateTime.UtcNow;
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
            var consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(q => q.statusConsulting != statusConsulting.Cancelled && q.statusConsulting != statusConsulting.UserRequestedNotPaid, include: q => q
                .Include(c => c.subConsulting)
                .Include(c => c.Lawyer)
                .Include(c => c.Customer)
                .Include(c => c.Files));

            var consultingDTOs = _mapper.Map<IEnumerable<ConsultingDTO>>(consultings);
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

            return consultingDTOs.OrderByDescending(q => q.OrderNumber).ToList();
        }

        public async Task<IEnumerable<ConsultingDTO>> GetConsultingsByCustomerIdAsync(string customerId)
        {
            // Retrieve consultings and related data from the database first
            var consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                a => a.CustomerId == customerId && a.statusConsulting != statusConsulting.Cancelled && a.statusConsulting != statusConsulting.UserRequestedNotPaid && !a.IsDeleted,
                include: q => q
                    .Include(c => c.subConsulting)
                    .Include(c => c.Lawyer)
                    .Include(c => c.Customer)
                    .Include(c => c.Files)
            );

            // Map consulting entities to DTOs
            var consultingDTOs = _mapper.Map<List<ConsultingDTO>>(consultings);

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

            return consultingDTOs.OrderByDescending(q => q.OrderNumber);
        }

        public async Task<IEnumerable<ConsultingDTO>> GetConsultingsInprogress(string id, bool isLawyer = false)
        {
            IEnumerable<Consulting> consultings = new List<Consulting>();
            if (isLawyer)
            {
                // Retrieve consultings and related data from the database first
                consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                    a => a.LawyerId == id && a.statusConsulting == statusConsulting.InProgress && !a.IsDeleted && !a.subConsulting.MainConsulting.service,
                    include: q => q
                        .Include(c => c.subConsulting).ThenInclude(c => c.MainConsulting)
                        .Include(c => c.Lawyer)
                        .Include(c => c.Customer)
                        .Include(c => c.Files)
                );
            }
            else
            {
                // Retrieve consultings and related data from the database first
                consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                    a => a.CustomerId == id && a.statusConsulting == statusConsulting.InProgress && !a.IsDeleted && !a.subConsulting.MainConsulting.service,
                    include: q => q
                        .Include(c => c.subConsulting).ThenInclude(c => c.MainConsulting)
                        .Include(c => c.Lawyer)
                        .Include(c => c.Customer)
                        .Include(c => c.Files)
                );
            }

            // Map consulting entities to DTOs
            var consultingDTOs = _mapper.Map<List<ConsultingDTO>>(consultings);

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
                foreach (var consulting1 in consultings.ToList())
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

            return consultingDTOs.OrderByDescending(q => q.OrderNumber);
        }

        public async Task<IEnumerable<ConsultingDTO>> GetConsultingsCompleted(string id, bool isLawyer = false)
        {
            IEnumerable<Consulting> consultings = new List<Consulting>();
            // Retrieve consultings and related data from the database first
            if (isLawyer)
            {
                consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                    a => a.LawyerId == id && a.statusConsulting == statusConsulting.Completed && !a.IsDeleted && !a.subConsulting.MainConsulting.service,
                    include: q => q
                        .Include(c => c.subConsulting).ThenInclude(c => c.MainConsulting)
                        .Include(c => c.Lawyer)
                        .Include(c => c.Customer)
                        .Include(c => c.Files)
                        .Include(c => c.Reviews)
                );
            }
            else
            {
                consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                    a => a.CustomerId == id && a.statusConsulting == statusConsulting.Completed && !a.IsDeleted && !a.subConsulting.MainConsulting.service,
                    include: q => q
                        .Include(c => c.subConsulting).ThenInclude(c => c.MainConsulting)
                        .Include(c => c.Lawyer)
                        .Include(c => c.Customer)
                        .Include(c => c.Files)
                        .Include(c => c.Reviews)
                );
            }

            // Map consulting entities to DTOs
            var consultingDTOs = _mapper.Map<List<ConsultingDTO>>(consultings);

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
                foreach (var consulting1 in consultings.ToList())
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

            return consultingDTOs.OrderByDescending(q => q.OrderNumber);
        }
    
        public async Task<IEnumerable<ConsultingDTO>> GetConsultingsCancelled(string customerId)
        {
            // Retrieve consultings and related data from the database first
            var consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                a => a.CustomerId == customerId && a.statusConsulting == statusConsulting.Cancelled && !a.IsDeleted && !a.subConsulting.MainConsulting.service,
                include: q => q
                    .Include(c => c.subConsulting).ThenInclude(c => c.MainConsulting)
                    .Include(c => c.Lawyer)
                    .Include(c => c.Customer)
                    .Include(c => c.Files)
            );

            // Map consulting entities to DTOs
            var consultingDTOs = _mapper.Map<List<ConsultingDTO>>(consultings);

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

            return consultingDTOs.OrderByDescending(q=>q.OrderNumber);
        }
        public async Task<IEnumerable<ConsultingDTO>> GetConsultingsbyStatus(statusConsulting statusConsulting)
        {
            // Retrieve consultings and related data from the database first
            var consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                a => a.statusConsulting == statusConsulting && !a.IsDeleted && !a.subConsulting.MainConsulting.service,
                include: q => q
                    .Include(c => c.subConsulting).ThenInclude(c => c.MainConsulting)
                    .Include(c => c.Lawyer)
                    .Include(c => c.Customer)
                    .Include(c => c.Files)
            );

            // Map consulting entities to DTOs
            var consultingDTOs = _mapper.Map<List<ConsultingDTO>>(consultings);

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

            return consultingDTOs.OrderByDescending(q=>q.OrderNumber);
        }

        public async Task<IEnumerable<ConsultingDTO>> GetServicesInprogress(string customerId)
        {
            // Retrieve consultings and related data from the database first
            var consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                a => a.CustomerId == customerId && a.statusConsulting == statusConsulting.InProgress,
                include: q => q
                    .Include(c => c.subConsulting).ThenInclude(c => c.MainConsulting)
                    .Include(c => c.Lawyer)
                    .Include(c => c.Customer)
                    .Include(c => c.Files)
            );

            // Map consulting entities to DTOs
            var consultingDTOs = _mapper.Map<List<ConsultingDTO>>(consultings.Where(c => !c.IsDeleted && c.subConsulting.MainConsulting.service));

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

            return consultingDTOs.OrderByDescending(q=>q.OrderNumber);
        }

        public async Task<IEnumerable<ConsultingDTO>> GetServicesCompleted(string customerId)
        {
            // Retrieve consultings and related data from the database first
            var consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                a => a.CustomerId == customerId && a.statusConsulting == statusConsulting.Completed && !a.IsDeleted && a.subConsulting.MainConsulting.service,
                include: q => q
                    .Include(c => c.subConsulting).ThenInclude(c=>c.MainConsulting)
                    .Include(c => c.Lawyer)
                    .Include(c => c.Customer)
                    .Include(c => c.Files)
            );

            // Map consulting entities to DTOs
            var consultingDTOs = _mapper.Map<List<ConsultingDTO>>(consultings);

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

            return consultingDTOs.OrderByDescending(q=>q.OrderNumber);
        }

        public async Task<IEnumerable<ConsultingDTO>> GetServicesCancelled(string customerId)
        {
            // Retrieve consultings and related data from the database first
            var consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                a => a.CustomerId == customerId && a.statusConsulting == statusConsulting.Cancelled && !a.IsDeleted && a.subConsulting.MainConsulting.service,
                include: q => q
                    .Include(c => c.subConsulting).ThenInclude(c => c.MainConsulting)
                    .Include(c => c.Lawyer)
                    .Include(c => c.Customer)
                    .Include(c => c.Files)
            );

            // Map consulting entities to DTOs
            var consultingDTOs = _mapper.Map<List<ConsultingDTO>>(consultings);

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

            return consultingDTOs.OrderByDescending(q=>q.OrderNumber);
        }

        public async Task<IEnumerable<ConsultingDTO>> GetServices(string customerId)
        {
            // Retrieve consultings and related data from the database first
            var consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                a => a.CustomerId == customerId && a.statusConsulting==statusConsulting.UserRequestedNotPaid && !a.IsDeleted && a.subConsulting.MainConsulting.service,
                include: q => q
                    .Include(c => c.subConsulting).ThenInclude(c => c.MainConsulting)
                    .Include(c => c.Lawyer)
                    .Include(c => c.Customer)
                    .Include(c => c.Files)
            );

            // Map consulting entities to DTOs
            var consultingDTOs = _mapper.Map<List<ConsultingDTO>>(consultings);

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
                consulting.numberRequest = _unitOfWork.RequestConsultingRepository.Count(q => q.ConsultingId == consulting.Id);
            }
            return consultingDTOs.OrderByDescending(q=>q.OrderNumber);
        }

        public async Task<IEnumerable<ConsultingDTO>> GetConsultingsByLawyerIdAsync(string lawyerId, statusConsulting status)
        {
            var consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                a => a.statusConsulting == status && a.statusConsulting != statusConsulting.Cancelled && !a.IsDeleted,
                include: q => q
                    .Include(c => c.subConsulting)
                    .Include(c => c.Lawyer)
                    .Include(c => c.Customer)
                    .Include(c => c.Files)
            );

            var consultingDTOs = _mapper.Map<IEnumerable<ConsultingDTO>>(consultings);

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

            return consultingDTOs.OrderByDescending(q=>q.OrderNumber).ToList();
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
            if (consulting == null) throw new ArgumentException("لم يتم العثور على الاستشارة");
            if (statusConsulting.InProgress == status) consulting.EndDate = DateTime.UtcNow;
            consulting.statusConsulting = status;
            consulting.IsUpdated = true; 
            consulting.UpdatedAt = DateTime.Now;

            _unitOfWork.ConsultingRepository.Update(consulting);
            await _unitOfWork.SaveChangesAsync();

            if (status == statusConsulting.InProgress) {
                if (consulting.LawyerId is not null)
                {
                    await _notificationService.SaveNotificationAsync(new SaveNotificationDTO
                    {
                        UserId = consulting.LawyerId,
                        NotificationType = NotificationType.ConsultationStarted,
                        ActionId = consulting.Id
                    });
                }

                if (consulting.CustomerId is not null)
                {
                    await _notificationService.SaveNotificationAsync(new SaveNotificationDTO
                    {
                        UserId = consulting.CustomerId,
                        NotificationType = NotificationType.ConsultationStarted,
                        ActionId = consulting.Id
                    });
                }
            }
            else if (status == statusConsulting.Cancelled) {
                if (consulting.LawyerId is not null)
                {
                    await _notificationService.SaveNotificationAsync(new SaveNotificationDTO
                    {
                        UserId = consulting.LawyerId,
                        NotificationType = NotificationType.ConsultationCancelled,
                        ActionId = consulting.Id
                    });
                }

                if (consulting.CustomerId is not null)
                {
                    await _notificationService.SaveNotificationAsync(new SaveNotificationDTO
                    {
                        UserId = consulting.CustomerId,
                        NotificationType = NotificationType.ConsultationCancelled,
                        ActionId = consulting.Id
                    });
                }
            }
        }

        public async Task PaymentConsulting(TransactionDTO transactionDTO)
        {
            if (transactionDTO == null) throw new ArgumentNullException(nameof(transactionDTO));

            var transaction = new Core.Entity.ConsultingData.Transaction
            {
                ConsultingId = transactionDTO.ConsultingId,
                PaymentId = transactionDTO.PaymentId,
                Amount = transactionDTO.Amount,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
           
            _unitOfWork.TransactionRepository.Add(transaction);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ConsultingDTO>> GetAvailableConsultations(string lawyerId)
        {
            List<string> ignoredConsultationIds = (await _unitOfWork.IgnoredConsultationsRepository
                .FindAllAsync(i => i.LawyerId == lawyerId))
                .Select(i => i.consultingId)
                .ToList();


            var consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                c => c.LawyerId == null && !c.IsDeleted && !c.subConsulting.MainConsulting.service && c.LawyerId == null && c.statusConsulting == statusConsulting.UserRequestedNotPaid && !ignoredConsultationIds.Contains(c.Id),
                include: q => q
                    .Include(c => c.subConsulting)
                    .Include(c => c.Customer)
                    .Include(c => c.Files)
            );

            var consultingDTOs = _mapper.Map<IEnumerable<ConsultingDTO>>(consultings);

            var allFiles = consultings
            .SelectMany(c => c.Files.Select(file => file.Id))
            .ToList();

            var fileUrls = await _fileHandling.GetAllFiles(allFiles);

            // After completing all database operations, fetch profile images asynchronously
            foreach (var consulting in consultingDTOs)
            {
                if (consulting.Customer != null && !string.IsNullOrEmpty(consulting.Customer.ProfileImageId))
                {
                    consulting.Customer.ProfileImage = await _accountService.GetUserProfileImage(consulting.Customer.ProfileImageId);
                }

                var matchingConsulting = consultings.FirstOrDefault(c => c.Id == consulting.Id);

                consulting.FilesUrl = matchingConsulting?
                    .Files.Select(file => fileUrls.TryGetValue(file.Id, out var fileUrl) ? fileUrl : null)
                    .ToList() ?? new List<string>();

            }


            return consultingDTOs.OrderByDescending(q=>q.OrderNumber).ToList();
        }

        public async Task AcceptConsultation(string lawyerId, string consultationId)
        {
            var consulting = await _unitOfWork.ConsultingRepository.GetByIdAsync(consultationId);
            
            if (consulting is null) throw new ArgumentException("لم يتم العثور على الاستشارة");
            if (consulting.LawyerId is not null) throw new ArgumentException("تم قبول الاستشارة بالفعل");

            consulting.LawyerId = lawyerId;
            consulting.Lawyer = await _accountService.GetUserById(lawyerId);
            consulting.PriceService = consulting.Lawyer.PriceService is null ? 0 : (decimal)((consulting.voiceConsulting) ? 100 : consulting.Lawyer.PriceService);
            consulting.StartDate = DateTime.UtcNow;
            consulting.statusConsulting = statusConsulting.InProgress;
            consulting.IsUpdated = true;
            consulting.UpdatedAt = DateTime.Now;

            _unitOfWork.ConsultingRepository.Update(consulting);
            await _unitOfWork.SaveChangesAsync();


                await _notificationService.SaveNotificationAsync(new SaveNotificationDTO
                {
                    UserId = consulting.CustomerId,
                    NotificationType = NotificationType.ConsultationStarted,
                    ActionId = consulting.Id
                });
            
        }

        public async Task<IEnumerable<ConsultingDTO>> GetAvailableServices()
        {
            var consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                c => c.LawyerId == null && !c.IsDeleted && c.subConsulting.MainConsulting.service && c.LawyerId == null && c.statusConsulting == statusConsulting.UserRequestedNotPaid,
                include: q => q
                    .Include(c => c.subConsulting).ThenInclude(c => c.MainConsulting)
                    .Include(c => c.Customer)
                    .Include(c => c.Files)
            );

            var consultingDTOs = _mapper.Map<IEnumerable<ConsultingDTO>>(consultings);


            var allFiles = consultings
            .SelectMany(c => c.Files.Select(file => file.Id))
            .ToList();

            var fileUrls = await _fileHandling.GetAllFiles(allFiles);

            // After completing all database operations, fetch profile images asynchronously
            foreach (var consulting in consultingDTOs)
            {
                if (consulting.Customer != null && !string.IsNullOrEmpty(consulting.Customer.ProfileImageId))
                {
                    consulting.Customer.ProfileImage = await _accountService.GetUserProfileImage(consulting.Customer.ProfileImageId);
                }

                var matchingConsulting = consultings.FirstOrDefault(c => c.Id == consulting.Id);

                consulting.FilesUrl = matchingConsulting?
                    .Files.Select(file => fileUrls.TryGetValue(file.Id, out var fileUrl) ? fileUrl : null)
                    .ToList() ?? new List<string>();
            }


            return consultingDTOs.ToList();
        }

        public async Task<IEnumerable<ConsultingDTO>> GetRequestConsultings(string lawyerId, statusConsulting status)
        {
            IEnumerable<Consulting> consultings = new List<Consulting>();

            // Retrieve consultings and related data from the database first
            consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                a => a.LawyerId == lawyerId &&
                a.statusConsulting == status &&
                !a.IsDeleted &&
                a.subConsulting.MainConsulting.service,
            include: q => q
                .Include(c => c.subConsulting).ThenInclude(c => c.MainConsulting)
                .Include(c => c.Lawyer)
                .Include(c => c.Customer)
                .Include(c => c.Files)
                .Include(c => c.Reviews)
            );



            // Map consulting entities to DTOs
            var consultingDTOs = _mapper.Map<List<ConsultingDTO>>(consultings);

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

                consulting.SubConsultingName = _unitOfWork.SubConsultingRepository.GetByIdAsync(consulting.SubConsultingId).Result.Name;
                consulting.FilesUrl = new List<string>();
                foreach (var consulting1 in consultings.ToList())
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

            return consultingDTOs.OrderByDescending(q => q.OrderNumber);
        }

        public async Task<IEnumerable<ConsultingDTO>> GetOfferedRequestConsultingsAsync(string lawyerId, statusRequestConsulting requestStatus)
        {
            IEnumerable<Consulting> consultings = new List<Consulting>();

            // Retrieve consultings and related data from the database first
            consultings = await _unitOfWork.ConsultingRepository.FindAllAsync(
                a => a.LawyerId == null &&
                a.statusConsulting == statusConsulting.UserRequestedNotPaid &&
                !a.IsDeleted && a.subConsulting.MainConsulting.service &&
                a.RequestConsultings.Any(rc => rc.LawyerId == lawyerId && rc.statusRequestConsulting == requestStatus),
            include: q => q
                .Include(c => c.RequestConsultings)
                .Include(c => c.subConsulting).ThenInclude(c => c.MainConsulting)
                .Include(c => c.Lawyer)
                .Include(c => c.Customer)
                .Include(c => c.Files)
            );



            // Map consulting entities to DTOs
            var consultingDTOs = _mapper.Map<List<ConsultingDTO>>(consultings);

            // After completing all database operations, fetch profile images asynchronously
            foreach (var consulting in consultingDTOs)
            {
                var lawyerRequests = consulting.RequestConsultings?.Where(c => c.LawyerId == lawyerId).ToList();
                if (lawyerRequests.Any()) {
                    var lawyerRequest = lawyerRequests.FirstOrDefault();
                    consulting.RequestConsultings = new List<RequestConsultingDTO>
                    {
                        new RequestConsultingDTO
                        {
                            Id = lawyerRequest.Id,
                            Description = lawyerRequest.Description,
                            PriceService = lawyerRequest.PriceService
                        }
                    };
                }

                if (consulting.Lawyer != null && !string.IsNullOrEmpty(consulting.Lawyer.ProfileImageId))
                {
                    consulting.Lawyer.ProfileImage = await _accountService.GetUserProfileImage(consulting.Lawyer.ProfileImageId);
                }

                if (consulting.Customer != null && !string.IsNullOrEmpty(consulting.Customer.ProfileImageId))
                {
                    consulting.Customer.ProfileImage = await _accountService.GetUserProfileImage(consulting.Customer.ProfileImageId);
                }

                consulting.SubConsultingName = _unitOfWork.SubConsultingRepository.GetByIdAsync(consulting.SubConsultingId).Result.Name;
                consulting.FilesUrl = new List<string>();
                foreach (var consulting1 in consultings.ToList())
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

            return consultingDTOs.OrderByDescending(q => q.OrderNumber);
        }

        public async Task IgnoreConsultationAsync(string lawyerId, string consultingId)
        {
            var ignored = new IgnoredConsultation
            {
                LawyerId = lawyerId,
                consultingId = consultingId,
            };

            await _unitOfWork.IgnoredConsultationsRepository.AddAsync(ignored);
            await _unitOfWork.SaveChangesAsync();
        }


    }

}
