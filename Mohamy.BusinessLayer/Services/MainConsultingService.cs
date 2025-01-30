using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Entity.ConsultingData;
using Mohamy.RepositoryLayer.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Mohamy.BusinessLayer.Services
{
    public class MainConsultingService : IMainConsultingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileHandling _fileHandling;
        private readonly IAccountService _accountService;

        public MainConsultingService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
            _accountService = accountService;
        }

        // Get all MainConsultings
        public async Task<List<MainConsultingDTO>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.MainConsultingRepository.GetAllAsync(include: q => q.Include(s => s.SubConsultings));
                entities = entities.Where(e => !e.IsDeleted).ToList();

                var dtos = new List<MainConsultingDTO>();

                foreach (var entity in entities)
                {
                    var dto = new MainConsultingDTO
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description,
                        IconUrl = await _fileHandling.GetFile(entity.iconId),
                        service = entity.service,
                    };

                    dtos.Add(dto);
                }

                return dtos;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("حدث خطأ أثناء استرجاع الاستشارات الرئيسية", ex);
            }
        }

        // Get MainConsulting by ID
        public async Task<MainConsultingDTO> GetByIdAsync(string id)
        {
            try
            {
                var entity = await _unitOfWork.MainConsultingRepository.FindAsync(a => a.Id == id, include: q => q.Include(s => s.SubConsultings));
                if (entity == null) return null;

                var dto = new MainConsultingDTO
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    IconUrl = await _fileHandling.GetFile(entity.iconId),
                    service = entity.service,
                };

                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"{id} حدث خطأ أثناء استرداد الاستشارة الرئيسية بالمعرف", ex);
            }
        }

        // Add a new MainConsulting
        public async Task<int> AddAsync(MainConsultingDTO dto)
        {
            try
            {
                // Check if DTO is populated properly
                if (dto == null)
                {
                    throw new ApplicationException("DTO is null.");
                }

                // Map DTO to entity
                var entity = _mapper.Map<mainConsulting>(dto);

                if (entity == null)
                {
                    throw new ApplicationException("Mapping from DTO to entity failed.");
                }

                var path = await _accountService.GetPathByName("mainConsultingIcon");

                if (dto.Icon != null)
                {
                    var iconEntity = await _fileHandling.UploadFile(dto.Icon, path);
                    entity.iconId = iconEntity;
                }

                await _unitOfWork.MainConsultingRepository.AddAsync(entity);
                return await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the error to understand it better
                throw new ApplicationException("حدث خطأ أثناء إضافة الاستشارة الرئيسية", ex);
            }
        }

        // Update an existing MainConsulting
        public async Task<int> UpdateAsync(MainConsultingDTO dto)
        {
            try
            {
                var entity = await _unitOfWork.MainConsultingRepository.GetByIdAsync(dto.Id);
                if (entity != null)
                {
                    var path = await _accountService.GetPathByName("mainConsultingIcon");
                    if (dto.Icon != null)
                    {
                        var iconEntity = await _fileHandling.UpdateFile(dto.Icon, path, entity.iconId);
                        entity.iconId = iconEntity;
                    }

                    _mapper.Map(dto, entity);

                    entity.IsUpdated = true;
                    entity.UpdatedAt = DateTime.Now;

                    _unitOfWork.MainConsultingRepository.Update(entity);
                    return await _unitOfWork.SaveChangesAsync();
                }
                return 0; // No update performed
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"{dto.Id} حدث خطأ أثناء تحديث الاستشارة الرئيسية بالمعرف", ex);
            }
        }

        // Delete a MainConsulting
        public async Task<int> DeleteAsync(string id)
        {
            try
            {
                var mainConsulting = await _unitOfWork.MainConsultingRepository.GetByIdAsync(id);
                if (mainConsulting != null)
                {
                    mainConsulting.IsDeleted = true;
                    mainConsulting.DeletedAt = DateTime.Now;

                    _unitOfWork.MainConsultingRepository.Update(mainConsulting);
                    return await _unitOfWork.SaveChangesAsync();
                }
                return 0; // No delete performed
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"{id} حدث خطأ أثناء حذف الاستشارة الرئيسية بالمعرف", ex);
            }
        }
    }
}
