using Mohamy.Core.DTO.AuthViewModel.RegisterModel;
using Mohamy.Core.DTO.ConsultingViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.Core.DTO.AuthViewModel.UpdateModel
{
    public class ManageLawyerViewModel
    {
        public ExperienceDTO? Experience { get; set; }
        public QualificationUser? qualificationUser { get; set; }
        public PersonalUpdate? personalUpdate { get; set; }
    }
}
