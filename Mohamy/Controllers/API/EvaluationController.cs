using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO;
using Mohamy.Core.DTO.EvaluationViewModel;
using System.Security.Claims;

namespace Mohamy.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EvaluationController : BaseController
    {
        private readonly IEvaluationService _evaluationService;
        public EvaluationController(IEvaluationService evaluationService)
        {
            _evaluationService = evaluationService;
        }

        [HttpPost]
        public async Task<IActionResult> AddEvaluation([FromBody] EvaluationDTO evaluation)
        {
            var evaluatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(evaluatorId)) return Unauthorized();

            await _evaluationService.AddEvaluationAsync(evaluatorId, evaluation);
            return Ok(new BaseResponse { status=true,
                Data = "Evaluation added successfully" });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetEvaluations(string userId)
        {
            var evaluations = await _evaluationService.GetEvaluationsAsync(userId);
            return Ok(new BaseResponse
            {
                status=true,
                Data = evaluations
            });
        }
    }
}
