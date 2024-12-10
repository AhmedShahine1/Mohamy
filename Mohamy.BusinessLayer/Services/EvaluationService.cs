using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.EvaluationViewModel;
using Mohamy.Core.Entity.Others;
using Mohamy.Core;
using Microsoft.EntityFrameworkCore;

namespace Mohamy.BusinessLayer.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly ApplicationDbContext _context;

        public EvaluationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddEvaluationAsync(string evaluatorId, EvaluationDTO evaluation)
        {
            var newEvaluation = new Evaluation
            {
                EvaluatorId = evaluatorId,
                EvaluatedId = evaluation.EvaluatedId,
                Rating = evaluation.Rating,
                Comment = evaluation.Comment
            };

            _context.Evaluations.Add(newEvaluation);
            await _context.SaveChangesAsync();
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
