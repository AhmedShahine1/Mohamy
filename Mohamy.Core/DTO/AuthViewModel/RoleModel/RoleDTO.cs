using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Mohamy.Core.DTO.AuthViewModel.RoleModel
{
    [DebuggerDisplay("{RoleName,nq}")]
    public class RoleDTO
    {        
        [Required, Display(Name = "Role Name"), StringLength(50)]
        public string RoleName { get; set; }
        [Required, Display(Name = "Role Description"), StringLength(int.MaxValue)]
        public string RoleDescription { get; set; }
        [Required, Display(Name = "Role Name Arabic"), StringLength(50)]
        public string RoleAr { get; set; }
    }
}
