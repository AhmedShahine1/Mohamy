using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.EvaluationViewModel;
using System.Security.Claims;

namespace Mohamy.Controllers.API
{
    public class EvaluationController : BaseController
    {
        private readonly IEvaluationService _evaluationService;

        public EvaluationController(IEvaluationService evaluationService)
        {
            _evaluationService = evaluationService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddEvaluation([FromBody] EvaluationDTO evaluation)
        {
            var evaluatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(evaluatorId)) return Unauthorized();

            await _evaluationService.AddEvaluationAsync(evaluatorId, evaluation);
            return Ok(new { Message = "Evaluation added successfully" });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetEvaluations(string userId)
        {
            var evaluations = await _evaluationService.GetEvaluationsAsync(userId);
            return Ok(evaluations);
        }
    }
}
