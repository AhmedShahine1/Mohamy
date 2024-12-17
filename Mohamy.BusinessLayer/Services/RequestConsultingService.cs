using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.AuthViewModel;
using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Entity.ConsultingData;
using Mohamy.Core.Helpers;
using Mohamy.RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.BusinessLayer.Services
{
    public class RequestConsultingService : IRequestConsultingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public RequestConsultingService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountService = accountService;
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
            return requestDTO;
        }

        public async Task<IEnumerable<RequestConsultingDTO>> GetRequestsByConsultingAsync(string consultingId)
        {
            var requests = (await _unitOfWork.RequestConsultingRepository
                .FindAllAsync(r => r.ConsultingId == consultingId && r.statusRequestConsulting != statusRequestConsulting.Cancel));
            return _mapper.Map<IEnumerable<RequestConsultingDTO>>(requests);
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
            return requestDTO;
        }

        public async Task<bool> UpdateRequestStatusAsync(string requestId, statusRequestConsulting newStatus)
        {
            var request = await _unitOfWork.RequestConsultingRepository.GetByIdAsync(requestId);
            if (request == null)
                throw new ArgumentException("Request not found");

            if (request.statusRequestConsulting == statusRequestConsulting.Waiting)
            {
                request.statusRequestConsulting = newStatus;

                if (newStatus == statusRequestConsulting.Approved)
                {
                    var consulting = await _unitOfWork.ConsultingRepository.GetByIdAsync(request.ConsultingId);
                    if (consulting != null)
                    {
                        consulting.LawyerId = request.LawyerId;
                        consulting.PriceService = request.PriceService;
                        _unitOfWork.ConsultingRepository.Update(consulting);
                    }
                }

                _unitOfWork.RequestConsultingRepository.Update(request);

                try
                {
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating request status: " + ex.InnerException?.Message, ex);
                }
            }

            return false;
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

            return _mapper.Map<RequestConsultingDTO>(requestConsult);
        }
    }
}
