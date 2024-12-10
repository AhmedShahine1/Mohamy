using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Entity.ConsultingData;
using Mohamy.RepositoryLayer.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mohamy.Core.Entity.ApplicationData;

namespace Mohamy.BusinessLayer.Services
{
    public class SubConsultingService : ISubConsultingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileHandling _fileHandling;
        private readonly IAccountService _accountService;
        private readonly IMainConsultingService _mainConsultingService;

        public SubConsultingService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling, IAccountService accountService, IMainConsultingService mainConsultingService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
            _accountService = accountService;
            _mainConsultingService = mainConsultingService;
        }

        // Get all SubConsultings
        public async Task<List<SubConsultingDTO>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.SubConsultingRepository.GetAllAsync(include: q => q.Include(s => s.MainConsulting));
                entities = entities.Where(e => !e.IsDeleted).ToList();

                var dtos = new List<SubConsultingDTO>();

                foreach (var entity in entities)
                {
                    var dto = new SubConsultingDTO
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description,
                        MainConsultingId = entity.MainConsultingId,
                        IconUrl = await _fileHandling.GetFile(entity.iconId),
                        mainConsulting = await _mainConsultingService.GetByIdAsync(entity.MainConsultingId)
                    };

                    dtos.Add(dto);
                }

                return dtos;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving sub consultings.", ex);
            }
        }

        public async Task<List<SubConsultingDTO>> GetSubConsultingByMainAsync(string mainConsulting)
        {
            try
            {
                var entities = await _unitOfWork.SubConsultingRepository.FindAllAsync(w => w.MainConsultingId == mainConsulting && !w.IsDeleted, include: q => q.Include(s => s.MainConsulting),isNoTracking:false);
                var dtos = new List<SubConsultingDTO>();

                foreach (var entity in entities)
                {
                    var dto = new SubConsultingDTO
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description,
                        MainConsultingId = entity.MainConsultingId,
                        IconUrl = await _fileHandling.GetFile(entity.iconId),
                        mainConsulting = await _mainConsultingService.GetByIdAsync(entity.MainConsultingId)
                    };

                    dtos.Add(dto);
                }

                return dtos;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving sub consultings.", ex);
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersBySubConsultingAsync(string subConsultingId)
        {
            if (string.IsNullOrEmpty(subConsultingId))
            {
                throw new ArgumentException("subConsultingId cannot be null or empty", nameof(subConsultingId));
            }

            // Fetch experiences that match the given subConsultingId
            var experiences = await _unitOfWork.ExperienceRepository
                .FindAllAsync(e => e.subConsultingId == subConsultingId, isNoTracking: true);

            if (!experiences.Any())
            {
                return Enumerable.Empty<ApplicationUser>();
            }

            // Extract Lawyer IDs from the experiences
            var LaywerIds = experiences.Select(e => e.LawyerId).Distinct().ToList();

            // Fetch users by Lawyer IDs
            var users = await _unitOfWork.UserRepository
                .FindAllAsync(u => LaywerIds.Contains(u.Id),include:query=>query
                .Include(u => u.Profile));

            return users;
        }

        // Get SubConsulting by ID
        public async Task<SubConsultingDTO> GetByIdAsync(string id)
        {
            try
            {
                var entity = await _unitOfWork.SubConsultingRepository.FindAsync(a => a.Id == id, include: q => q.Include(s => s.MainConsulting));
                if (entity == null) return null;

                var dto = new SubConsultingDTO
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    MainConsultingId = entity.MainConsultingId,
                    IconUrl = await _fileHandling.GetFile(entity.iconId),
                    mainConsulting = await _mainConsultingService.GetByIdAsync(entity.MainConsultingId)
                };

                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving sub consulting with ID {id}.", ex);
            }
        }

        // Add a new SubConsulting
        public async Task<int> AddAsync(SubConsultingDTO dto)
        {
            try
            {
                var entity = _mapper.Map<subConsulting>(dto);
                var path = await _accountService.GetPathByName("subConsultingIcon");

                if (dto.Icon != null)
                {
                    var iconEntity = await _fileHandling.UploadFile(dto.Icon, path);
                    entity.iconId = iconEntity;
                }

                await _unitOfWork.SubConsultingRepository.AddAsync(entity);
                return await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding sub consulting.", ex);
            }
        }

        // Update an existing SubConsulting
        public async Task<int> UpdateAsync(SubConsultingDTO dto)
        {
            try
            {
                var entity = await _unitOfWork.SubConsultingRepository.GetByIdAsync(dto.Id);
                if (entity != null)
                {
                    var path = await _accountService.GetPathByName("subConsultingIcon");

                    if (dto.Icon != null)
                    {
                        var iconEntity = await _fileHandling.UpdateFile(dto.Icon, path, entity.iconId);
                        entity.iconId = iconEntity;
                    }
                    _mapper.Map(dto, entity);
                    entity.IsUpdated = true;
                    entity.UpdatedAt = DateTime.Now;

                    _unitOfWork.SubConsultingRepository.Update(entity);
                    return await _unitOfWork.SaveChangesAsync();
                }
                return 0; // No update performed
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while updating sub consulting with ID {dto.Id}.", ex);
            }
        }

        // Delete a SubConsulting
        public async Task<int> DeleteAsync(string id)
        {
            try
            {
                var subConsulting = await _unitOfWork.SubConsultingRepository.GetByIdAsync(id);
                if (subConsulting != null)
                {
                    subConsulting.IsDeleted = true;
                    subConsulting.DeletedAt = DateTime.Now;

                    _unitOfWork.SubConsultingRepository.Update(subConsulting);
                    return await _unitOfWork.SaveChangesAsync();
                }
                return 0; // No delete performed
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting sub consulting with ID {id}.", ex);
            }
        }
    }
}
