using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.AuthViewModel;
using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.DTO.NotificationViewModel;
using Mohamy.Core.Entity.ConsultingData;
using Mohamy.Core.Helpers;
using Mohamy.RepositoryLayer.Interfaces;

namespace Mohamy.BusinessLayer.Services
{
    public class RequestConsultingService : IRequestConsultingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly INotificationService _notificationService;

        public RequestConsultingService(IUnitOfWork unitOfWork, INotificationService notificationService, IMapper mapper, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountService = accountService;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<RequestConsultingDTO>> GetAllRequestsAsync()
        {
            var requests = await _unitOfWork.RequestConsultingRepository.FindAllAsync(r => r.statusRequestConsulting != statusRequestConsulting.Cancel, include: q =>
            q.Include(a => a.Consulting)
            );
            var requestDTO = _mapper.Map<IEnumerable<RequestConsultingDTO>>(requests);
            foreach (var request in requestDTO)
            {
                request.Consulting.Customer = _mapper.Map<AuthDTO>(await _accountService.GetUserById(request.Consulting.CustomerId));
                request.Lawyer = _mapper.Map<AuthDTO>(await _accountService.GetUserById(request.LawyerId));
            }
            return requestDTO.OrderByDescending(q=>q.CreatedAt);
        }

        public async Task<IEnumerable<RequestConsultingDTO>> GetRequestsByConsultingAsync(string consultingId)
        {
            var requests = (await _unitOfWork.RequestConsultingRepository
                .FindAllAsync(r => r.ConsultingId == consultingId && r.statusRequestConsulting != statusRequestConsulting.Cancel));
            var requestDTO = _mapper.Map<IEnumerable<RequestConsultingDTO>>(requests);
            foreach (var request in requestDTO)
            {
                request.Lawyer = _mapper.Map<AuthDTO>(await _accountService.GetUserById(request.LawyerId));
                request.Lawyer.ProfileImage = await _accountService.GetUserProfileImage(request.Lawyer.ProfileImageId);
            }
            return requestDTO.OrderByDescending(q => q.CreatedAt);
        }

        public async Task<IEnumerable<RequestConsultingDTO>> GetRequestsByUserAsync(string userId)
        {
            var requests = await _unitOfWork.RequestConsultingRepository.FindAllAsync(r => r.LawyerId == userId && r.statusRequestConsulting != statusRequestConsulting.Cancel, include: q =>
            q.Include(a => a.Consulting));
            var requestDTO = _mapper.Map<IEnumerable<RequestConsultingDTO>>(requests);
            foreach (var request in requestDTO)
            {
                request.Consulting.Customer = _mapper.Map<AuthDTO>(await _accountService.GetUserById(request.Consulting.CustomerId));
            }
            return requestDTO.OrderByDescending(q => q.CreatedAt);
        }

        public async Task<bool> UpdateRequestStatusAsync(string requestId, statusRequestConsulting newStatus)
        {
            var request = await _unitOfWork.RequestConsultingRepository.GetByIdAsync(requestId);
            if (request == null)
                throw new ArgumentException("Request not found");

                request.statusRequestConsulting = newStatus;

                if (newStatus == statusRequestConsulting.Approved)
                {
                    var consulting = await _unitOfWork.ConsultingRepository.GetByIdAsync(request.ConsultingId);
                    if (consulting != null)
                    {
                        consulting.LawyerId = request.LawyerId;
                        consulting.PriceService = request.PriceService;
                        consulting.StartDate = DateTime.UtcNow;
                        consulting.statusConsulting = statusConsulting.InProgress;
                        consulting.IsUpdated = true;
                        consulting.UpdatedAt = DateTime.Now;
                    _unitOfWork.ConsultingRepository.Update(consulting);
                    }
                }

                _unitOfWork.RequestConsultingRepository.Update(request);

                try
                {
                    await _unitOfWork.SaveChangesAsync();

                    if (newStatus == statusRequestConsulting.Approved || newStatus == statusRequestConsulting.Rejection)
                    {
                        var consulting = await _unitOfWork.ConsultingRepository.GetByIdAsync(request.ConsultingId);
                        if (consulting is not null)
                        {
                            await _notificationService.SaveNotificationAsync(new SaveNotificationDTO
                            {
                                UserId = consulting.LawyerId,
                                NotificationType = newStatus == statusRequestConsulting.Approved ? NotificationType.OfferApproved: NotificationType.OfferRejected,
                                ActionId = consulting.Id
                            });
                        }
                    }
                    
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating request status: " + ex.InnerException?.Message, ex);
                }
        }

        public async Task<RequestConsultingDTO> AddRequestAsync(RequestConsultingDTO requestConsultingDTO)
        {
            var check = await _unitOfWork.RequestConsultingRepository.FindAsync(q => q.ConsultingId == requestConsultingDTO.ConsultingId && q.LawyerId == requestConsultingDTO.LawyerId);
            if (check != null)
            {
                return _mapper.Map<RequestConsultingDTO>(check);
            }
            var requestConsult = _mapper.Map<RequestConsulting>(requestConsultingDTO);

            await _unitOfWork.RequestConsultingRepository.AddAsync(requestConsult);
            await _unitOfWork.SaveChangesAsync();

            var consulting = await _unitOfWork.ConsultingRepository.FindAsync(c => c.Id == requestConsultingDTO.ConsultingId);
            await _notificationService.SaveNotificationAsync(new SaveNotificationDTO
            {
                UserId = consulting.CustomerId,
                NotificationType = NotificationType.OfferReceived,
                ActionId = requestConsultingDTO.ConsultingId
            });

            return _mapper.Map<RequestConsultingDTO>(requestConsult);
        }
    }
}
