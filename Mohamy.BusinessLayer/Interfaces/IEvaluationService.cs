using Mohamy.Core.DTO.EvaluationViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface IEvaluationService
    {
        Task AddEvaluationAsync(string evaluatorId, EvaluationDTO evaluation);
        Task<IEnumerable<EvaluationDetailsDTO>> GetEvaluationsAsync(string userId);
    }
}
