using Mohamy.Core.DTO.AuthViewModel;
using Mohamy.Core.DTO.AuthViewModel.RegisterModel;
using Mohamy.Core.DTO.AuthViewModel.RoleModel;
using Mohamy.Core.Entity.ApplicationData;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Mohamy.Core.Entity.Files;
using Mohamy.Core.DTO.AuthViewModel.UpdateModel;
using Mohamy.Core.Entity.LawyerData;

namespace Mohamy.BusinessLayer.Interfaces;

public interface IAccountService
{
    Task<ApplicationUser> GetUserById(string id);
    Task<IdentityResult> RegisterAdmin(RegisterAdmin model);
    Task<IdentityResult> UpdateAdmin(string adminId, RegisterAdmin model);
    Task<IdentityResult> RegisterSupportDeveloper(RegisterSupportDeveloper model);
    Task<IdentityResult> UpdateSupportDeveloper(string SupportDeveloperId, RegisterSupportDeveloper model);
    Task<IdentityResult> RegisterCustomer(RegisterCustomer model);
    Task<IdentityResult> UpdateCustomer(string adminId, UpdateCustomer model);
    Task<IdentityResult> UpdatePasswordAsync(string userId, UpdatePassword updatePasswordModel);
    Task<(bool IsSuccess, string Token, string ErrorMessage)> Login(LoginModel model);
    Task<bool> Logout(ApplicationUser user);
    Task<IEnumerable<Specialties>> GetAllSpecialtiesAsync(string userId);
    Task<IEnumerable<Experience>> GetAllExperiencesAsync(string userId);
    Task<IEnumerable<lawyerLicense>> GetAllLawyerLicensesAsync(string userId);
    Task<IEnumerable<graduationCertificate>> GetAllGraduationCertificatesAsync(string userId);
    Task<List<LawyerDTO>> GetLawyersAsync(string? keyword, string? city, string? specialization, int? minYearsExperience, int? maxYearsExperience, string? sortBy);
    Task<bool> SendOTP(string customerEmail);
    Task<bool> ValidateOTP(string customerPhoneNumber, string OTPV);
    Task<ApplicationUser> GetUserFromToken(string token);
    Task<string> AddRoleAsync(RoleUserModel model);
    Task<List<string>> GetRoles();
    Task<string> GetUserProfileImage(string profileId);
    Task<Paths> GetPathByName(string name);
    string ValidateJwtToken(string token);
    int GenerateRandomNo();
    ////------------------------------------------------------
    Task<IdentityResult> Activate(string userId);
    Task<IdentityResult> Suspend(string userId);
    //string RandomString(int length);
    //Task<bool> DisActiveUserConnnection(string userId);
    //Task<bool> ActiveUserConnnection(string userId);
}