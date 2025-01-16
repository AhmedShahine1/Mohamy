using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.EvaluationViewModel;
using Mohamy.Core.Entity.Others;
using Mohamy.Core;
using Microsoft.EntityFrameworkCore;
using Mohamy.RepositoryLayer.Interfaces;
using Mohamy.Core.DTO.NotificationViewModel;
using Mohamy.Core.Helpers;

namespace Mohamy.BusinessLayer.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public EvaluationService(IUnitOfWork unitOfWork,  ApplicationDbContext context, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _notificationService = notificationService;
        }

        public async Task AddEvaluationAsync(string evaluatorId, EvaluationDTO evaluation)
        {
            var consulting = await _unitOfWork.ConsultingRepository.GetByIdAsync(evaluation.ConsultingId);
            if (consulting == null) throw new ArgumentException("Consulting not found");


            consulting.Reviews = new List<Evaluation>{
                new Evaluation
                {
                    EvaluatorId = evaluatorId,
                    EvaluatedId = evaluation.EvaluatedId,
                    Rating = evaluation.Rating,
                    Comment = evaluation.Comment
                }
            };

            _unitOfWork.ConsultingRepository.Update(consulting);
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.SaveNotificationAsync(new SaveNotificationDTO
            {
                UserId = evaluation.EvaluatedId,
                NotificationType = NotificationType.NewRating,
                ActionId = evaluation.ConsultingId
            });
        }

        public async Task<IEnumerable<EvaluationDetailsDTO>> GetEvaluationsAsync(string userId)
        {
            return await _context.Evaluations
                .Where(e => e.EvaluatedId == userId)
                .Select(e => new EvaluationDetailsDTO
                {
                    EvaluatorId = e.EvaluatorId,
                    EvaluatorName = e.Evaluator.FullName,
                    Rating = e.Rating,
                    Comment = e.Comment,
                    Date = e.CreatedAt
                })
                .ToListAsync();
        }
    }
}
