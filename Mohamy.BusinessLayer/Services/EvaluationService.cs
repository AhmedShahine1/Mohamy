using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.EvaluationViewModel;
using Mohamy.Core.Entity.Others;
using Mohamy.Core;
using Microsoft.EntityFrameworkCore;
using Mohamy.RepositoryLayer.Interfaces;

namespace Mohamy.BusinessLayer.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public EvaluationService(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
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
