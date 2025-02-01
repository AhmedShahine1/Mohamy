using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.Entity.ApplicationData;
using Mohamy.Core.Entity.Files;
using Mohamy.Core.Helpers;
using Mohamy.RepositoryLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Mohamy.Core.DTO.AuthViewModel.RegisterModel;
using Mohamy.Core.DTO.AuthViewModel;
using Mohamy.Core.DTO.AuthViewModel.RoleModel;
using Microsoft.AspNetCore.Http;
using Mohamy.Core.DTO.AuthViewModel.UpdateModel;
using Mohamy.Core.Entity.LawyerData;
using System.Linq.Expressions;
using Mohamy.Core.DTO.CityViewModel;
using Microsoft.CodeAnalysis;
using Mohamy.Core.DTO.NotificationViewModel;

namespace Mohamy.BusinessLayer.Services;

public class AccountService : IAccountService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileHandling _fileHandling;
    private readonly Jwt _jwt;
    private readonly SmsSettings _smsSettings;
    private readonly IMemoryCache memoryCache;
    private readonly IMapper mapper;

    public AccountService(UserManager<ApplicationUser> userManager, IFileHandling photoHandling,
        RoleManager<ApplicationRole> roleManager, IUnitOfWork unitOfWork,
        IOptions<Jwt> jwt, IMemoryCache _memoryCache, IMapper _mapper, SignInManager<ApplicationUser> signInManager, IOptions<SmsSettings> smsSettings)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _jwt = jwt.Value;
        _fileHandling = photoHandling;
        memoryCache = _memoryCache;
        mapper = _mapper;
        _signInManager = signInManager;
        _smsSettings = smsSettings.Value;
    }
    //------------------------------------------------------------------------------------------------------------
    public async Task<ApplicationUser> GetUserById(string id)
    {
        var user = await _userManager.Users
            .Include(u => u.Profile)
            //.Include(u=>u.Experiences)
            .FirstOrDefaultAsync(x => x.Id == id && x.Status && !x.IsDeleted);
        return user;
    }
    //------------------------------------------------------------------------------------------------------------
    // Check if email or phone number already exists before creating or updating the user
    private async Task<bool> IsPhoneExistAsync(string phoneNumber, string userId = null, bool isLawyer = false)
    {
        var usersWithPhone = await _userManager.Users
        .Where(u => u.PhoneNumber == phoneNumber && u.Id != userId && !u.IsDeleted)
        .ToListAsync();

        // Check if any of the users have the specified role (lawyer) if required
        foreach (var user in usersWithPhone)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (isLawyer && (roles.Contains("Lawyer") || roles.Contains("Notary")))
            {
                return true;
            }

            if (!isLawyer && !roles.Contains("Lawyer") && !roles.Contains("Notary"))
            {
                return true;
            }
        }

        return false;
    }

    // Helper methods for handling profile images
    private async Task SetProfileImage(ApplicationUser user, IFormFile imageProfile)
    {
        var path = await GetPathByName("ProfileImages");

        if (imageProfile != null)
        {
            user.ProfileId = await _fileHandling.UploadFile(imageProfile, path);
        }
        else
        {
            user.ProfileId = await _fileHandling.DefaultProfile(path);
        }
    }

    private async Task UpdateProfileImage(ApplicationUser user, IFormFile imageProfile)
    {
        if (imageProfile != null)
        {
            var path = await GetPathByName("ProfileImages");
            try
            {
                user.ProfileId = await _fileHandling.UpdateFile(imageProfile, path, user.ProfileId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("فشل تحديث صورة الملف الشخصي", ex);
            }
        }
    }

    public async Task<ApplicationUser> GetUserFromToken(string token)
    {
        try
        {
            var userId = ValidateJwtToken(token);
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("لم يتم العثور على المستخدم");
            }
            return user;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }
    //------------------------------------------------------------------------------------------------------------
    // Register methods
    public async Task<IdentityResult> RegisterSupportDeveloper(RegisterSupportDeveloper model)
    {
        var user = mapper.Map<ApplicationUser>(model);

        if (model.ImageProfile != null)
        {
            var path = await GetPathByName("ProfileImages");
            user.ProfileId = await _fileHandling.UploadFile(model.ImageProfile, path);
        }
        else
        {
            var path = await GetPathByName("ProfileImages");
            user.ProfileId = await _fileHandling.DefaultProfile(path);
        }
        user.PhoneNumberConfirmed = true;
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Support Developer");
        }
        else
        {
            // Handle potential errors by throwing an exception or logging details
            throw new InvalidOperationException("فشل إنشاء المستخدم: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return result;
    }

    public async Task<IdentityResult> RegisterCustomer(RegisterCustomer model)
    {
        if (await IsPhoneExistAsync(model.PhoneNumber))
        {
            throw new ArgumentException("هذا الرقم لديه حساب بالفعل");
        }

        var user = mapper.Map<ApplicationUser>(model);
        await SetProfileImage(user, model.ImageProfile);
        user.PhoneNumberConfirmed = true;

        var result = await _userManager.CreateAsync(user, "Ahmed@123");

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Customer");
        }
        else
        {
            throw new InvalidOperationException($"فشل إنشاء المستخدم: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return result;
    }

    public async Task<IdentityResult> SetLawyerInitialDetail(string lawyerId, LawyerInitialDetail model)
    {
        var user = await _userManager.FindByIdAsync(lawyerId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المحامي");

        await SetProfileImage(user, model.ImageProfile);
        user.Description = model.Description;
        user.PriceService = model.PriceService;
        user.yearsExperience = model.YearsExperience;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"فشل تحديث المستخدم: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return result;
    }

    public async Task<IdentityResult> UpdateLawyer(string lawyerId, UpdateLawyer model)
    {
        var user = await _userManager.FindByIdAsync(lawyerId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المحامي");

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            throw new ArgumentException("كلمة المرور غير صحيحة");

        user.FullName = model.FullName;
        user.Email = model.Email;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"فشل تحديث المحامي: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return result;
    }

    public async Task<IdentityResult> SetUserOnlineOfflineStatusAsync(string userId, bool online)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المستخدم");

        user.Online = online;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"فشل تحديث المستخدم: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return result;
    }

    public async Task<IdentityResult> SaveUserDeviceAsync(string userId, SaveDeviceDTO saveDeviceDTO)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المستخدم");

        user.Device = saveDeviceDTO.DeviceId;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"فشل تحديث المستخدم: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        IEnumerable<ApplicationUser> userWithSameDevices = await _unitOfWork.UserRepository.FindAllAsync(u => u.Id != userId && u.Device == saveDeviceDTO.DeviceId);
        if (userWithSameDevices.Any())
        {
            foreach (var userWithDevice in userWithSameDevices)
            {
                userWithDevice.Device = null;
                _unitOfWork.UserRepository.Update(userWithDevice);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        return result;
    }

    public async Task<IdentityResult> UpdateLawyerProfile(string lawyerId, UpdateLawyerProfile model)
    {
        var user = await _userManager.FindByIdAsync(lawyerId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المحامي");

        user.Description = model.Description;
        user.yearsExperience = model.YearsExperience;
        user.AcademicQualification = model.AcademicQualification;
        user.City = model.City;
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            var licenses = await GetAllLawyerLicensesAsync(lawyerId);
            if (licenses.Any())
            {
                var primaryLicense = licenses.FirstOrDefault();
                primaryLicense.LicenseNumber = model.LicenseNumber;
                _unitOfWork.lawyerLicenseRepository.Update(primaryLicense);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                lawyerLicense license = new lawyerLicense()
                {
                    LicenseNumber = model.LicenseNumber,
                    LawyerId = lawyerId,
                    State = model.City
                };
                _unitOfWork.lawyerLicenseRepository.Add(license);
                await _unitOfWork.SaveChangesAsync();
            }

            if (model.Professions.Any())
            {
                var oldProfessions = await _unitOfWork.ProfessionsRepository.FindAllAsync(s => s.LawyerId == lawyerId);

                // Delete old professions
                if (oldProfessions != null && oldProfessions.Any())
                {
                    _unitOfWork.ProfessionsRepository.DeleteRange(oldProfessions);
                    await _unitOfWork.SaveChangesAsync();
                }

                var professions = model.Professions.Select(dto => new Profession
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    LawyerId = lawyerId
                }).ToList();

                _unitOfWork.ProfessionsRepository.AddRange(professions);
                await _unitOfWork.SaveChangesAsync();
            }

        }

        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"فشل تحديث ملف المحامي: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return result;
    }

    public async Task<IdentityResult> UpdateLawyerLanguages(string lawyerId, UpdateLanguages model)
    {
        var user = await _userManager.FindByIdAsync(lawyerId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المحامي");

        user.Languages = model.Languages;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"فشل تحديث لغة المحامي: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return result;
    }

    public async Task<IdentityResult> ChangeLawyerAvailability(string lawyerId)
    {
        var user = await _userManager.FindByIdAsync(lawyerId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المحامي");

        user.Available = !user.Available;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"فشل تحديث حالة توفر المحامي: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return result;
    }

    public async Task<IdentityResult> UpdateLawyerPhone(string lawyerId, UpdatePhone model)
    {
        var user = await _userManager.FindByIdAsync(lawyerId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المحامي");

        if (await IsPhoneExistAsync(model.PhoneNumber, null, true))
            throw new ArgumentException("رقم الهاتف موجود بالفعل");

        user.PhoneNumber = model.PhoneNumber;
        user.UserName = $"{model.PhoneNumber}_lawyer";
        user.NormalizedUserName = user.UserName.ToUpper();
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"فشل تحديث المحامي: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return result;
    }

    public async Task<IdentityResult> UpdateLawyerPrice(string lawyerId, UpdatePrice model)
    {
        var user = await _userManager.FindByIdAsync(lawyerId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المحامي");

        user.PriceService = model.PriceService;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"فشل تحديث المحامي: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return result;
    }

    public async Task<IdentityResult> UpdateLawyerSpecialities(string lawyerId, UpdateSpecialities model)
    {
        var user = await _userManager.FindByIdAsync(lawyerId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المحامي");

        var oldSpecialities = await _unitOfWork.SpecialtiesRepository.FindAllAsync(s => s.LawyerId == lawyerId);

        // Delete old specialities
        if (oldSpecialities != null && oldSpecialities.Any())
        {
            _unitOfWork.SpecialtiesRepository.DeleteRange(oldSpecialities);
            await _unitOfWork.SaveChangesAsync();
        }

        if (model.mainConsultingId != null && model.mainConsultingId.Any())
        {
            var newSpecialities = model.mainConsultingId.Select(id => new Specialties
            {
                LawyerId = lawyerId,
                mainConsultingId = id
            }).ToList();

            await _unitOfWork.SpecialtiesRepository.AddRangeAsync(newSpecialities);
            await _unitOfWork.SaveChangesAsync();
        }

        // Return successful result
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateLawyerBank(string lawyerId, UpdateBank model)
    {
        var user = await _userManager.FindByIdAsync(lawyerId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المحامي");

        user.BankName = model.BankName;
        user.BeneficiaryName = model.BeneficiaryName;
        user.AccountNumber = model.AccountNumber;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"فشل تحديث المحامي: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return result;
    }

    public async Task<IdentityResult> RegisterAdmin(RegisterAdmin model)
    {
        if (await IsPhoneExistAsync(model.PhoneNumber))
        {
            throw new ArgumentException("رقم الهاتف موجود بالفعل");
        }

        var user = mapper.Map<ApplicationUser>(model);
        await SetProfileImage(user, model.ImageProfile);
        user.PhoneNumberConfirmed = true;
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }
        else
        {
            throw new InvalidOperationException($"فشل إنشاء المستخدم: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return result;
    }

    public async Task DeleteProfessionAsync(string professionId)
    {
        var profession = await _unitOfWork.ProfessionsRepository.FindAsync(p => p.Id == professionId);
        if (profession is null)
            throw new ArgumentException("لم يتم العثور على المهنة");

        _unitOfWork.ProfessionsRepository.Delete(profession);
        await _unitOfWork.SaveChangesAsync();
    }

    //------------------------------------------------------------------------------------------------------------
    public async Task<IdentityResult> UpdateAdmin(string adminId, RegisterAdmin model)
    {
        var user = await _userManager.FindByIdAsync(adminId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المسؤول");

        if (await IsPhoneExistAsync(model.PhoneNumber, adminId))
        {
            throw new ArgumentException("رقم الهاتف موجود بالفعل");
        }

        user.FullName = model.FullName;
        user.PhoneNumber = model.PhoneNumber;

        await UpdateProfileImage(user, model.ImageProfile);

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> UpdateSupportDeveloper(string SupportDeveloperId, RegisterSupportDeveloper model)
    {
        var user = await _userManager.FindByIdAsync(SupportDeveloperId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المسؤول");

        user.FullName = model.FullName;

        if (model.ImageProfile != null)
        {
            var path = await GetPathByName("ProfileImages");
            try
            {
                var newProfileId = await _fileHandling.UpdateFile(model.ImageProfile, path, user.ProfileId);
                user.ProfileId = newProfileId;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"خطأ في تحديث الملف: {ex.Message}");
                throw new InvalidOperationException("فشل تحديث صورة الملف الشخصي", ex);
            }
        }

        try
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // Log errors
                Console.WriteLine($"خطأ في تحديث المستخدم: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                throw new InvalidOperationException($"خطأ في تحديث المستخدم: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            return result;
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"خطأ في تحديث المستخدم: {ex.Message}");
            throw new InvalidOperationException("فشل تحديث المسؤول", ex);
        }
    }

    public async Task<IdentityResult> UpdateCustomer(string customerId, UpdateCustomer model)
    {
        var user = await _userManager.FindByIdAsync(customerId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على العميل");

        if (await IsPhoneExistAsync(model.PhoneNumber, customerId))
        {
            throw new ArgumentException("رقم الهاتف موجود بالفعل");
        }

        user.FullName = model.FullName;
        user.PhoneNumber = model.PhoneNumber;

        await UpdateProfileImage(user, model.ImageProfile);

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> UpdatePasswordAsync(string userId, UpdatePassword updatePasswordModel)
    {
        if (updatePasswordModel == null)
            throw new ArgumentNullException(nameof(updatePasswordModel));

        // Validate user existence
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException("لم يتم العثور على المستخدم");

        // Validate password and confirmation (already done by Data Annotations)
        if (updatePasswordModel.Password != updatePasswordModel.ConfirmPassword)
            throw new ArgumentException("كلمة المرور وتأكيدها غير متطابقين");

        // Generate password reset token
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        // Reset the password
        var result = await _userManager.ResetPasswordAsync(user, resetToken, updatePasswordModel.Password);

        return result;
    }
    //------------------------------------------------------------------------------------------------------------
    public async Task<(bool IsSuccess, string Token, string ErrorMessage)> Login(LoginModel model)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password) || user.IsDeleted)
            {
                return (false, null, "محاولة تسجيل دخول غير صالحة");
            }

            // Check if user status is true and phone number is confirmed
            if (!user.Status)
            {
                return (false, null, "حسابك معطل. يرجى الاتصال بالدعم");
            }

            if (!user.PhoneNumberConfirmed)
            {
                return (false, null, "رقم الهاتف غير مؤكد. يرجى التحقق من رقم هاتفك");
            }

            // Proceed with login
            await _signInManager.SignInAsync(user, model.RememberMe);
            var token = await GenerateJwtToken(user, true);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return (true, tokenString, null);
        }
        catch (Exception ex)
        {
            return (false, null, ex.Message);
        }
    }

    public async Task<bool> Logout(ApplicationUser user)
    {
        if (user == null)
            return false;

        await _signInManager.SignOutAsync();
        return true;
    }

    public async Task<bool> SendOTP(string customerPhoneNumber)
    {
        // Generate a random OTP
        var OTP = RandomOTP(6);

        // Store OTP in memory cache
        memoryCache.Set(customerPhoneNumber, OTP);

        // Prepare the API URL
        var smsApiUrl = "https://www.dreams.sa/index.php/api/sendsms/";

        // Prepare the parameters for the POST request
        var smsParameters = new Dictionary<string, string>
{
    { "user", _smsSettings.sender },
    { "secret_key", _smsSettings.SecretKey },
    { "to", customerPhoneNumber },
    { "message", $"OTP :{OTP} \n محام | للاستشارات القانونية"
     },
    { "sender", "TASIA-IT" }
};

        try
        {
            using (var httpClient = new HttpClient())
            {
                // Send the POST request to the SMS API
                var smsContent = new FormUrlEncodedContent(smsParameters);
                var smsResponse = await httpClient.PostAsync(smsApiUrl, smsContent);

                if (smsResponse.IsSuccessStatusCode)
                {
                    var smsResponseContent = await smsResponse.Content.ReadAsStringAsync();
                    if (smsResponseContent.Contains("-124"))
                    {
                        // Handle the response for failure cases based on the API response
                        Console.WriteLine($"فشل إرسال الرسالة. الاستجابة: {smsResponseContent}");
                    }
                    else
                    {
                        return true; // SMS sent successfully
                    }
                }
                else
                {
                    Console.WriteLine($"فشل طلب الرسالة: {smsResponse.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"حدث استثناء: {ex.Message}");
        }

        return false; // Return false if SMS sending fails
    }

    public async Task<bool> ValidateOTP(string customerPhoneNumber, string OTPV)
    {
        try
        {
            return true;
            // Check if the OTP exists in the memory cache
            if (memoryCache.TryGetValue(customerPhoneNumber, out string cachedOTP))
            {
                // Compare the provided OTP with the cached OTP
                if (cachedOTP == OTPV)
                {
                    // OTP is valid, remove it from the cache after successful validation
                    memoryCache.Remove(customerPhoneNumber);
                    return true;
                }
            }

            // OTP is invalid or not found
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"حدث استثناء أثناء التحقق من OTP: {ex.Message}");
            return false;
        }
    }

    //------------------------------------------------------------------------------------------------------------

    public async Task<Paths> GetPathByName(string name)
    {
        var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
        if (path == null) throw new ArgumentException("لم يتم العثور على المسار");
        return path;
    }
    //------------------------------------------------------------------------------------------------------------
    public async Task<string> AddRoleAsync(RoleUserModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user is null)
            return "لم يتم العثور على البائع";

        if (model.RoleId != null && model.RoleId.Count() > 0)
        {
            var roleUser = _userManager.GetRolesAsync(user).Result;
            IEnumerable<string> roles = new List<string>();
            foreach (var roleid in model.RoleId)
            {
                var role = _roleManager.FindByIdAsync(roleid).Result.Name;
                if (roleUser.Contains(role))
                {
                    roles.Append(role);
                }
            }
            var result = await _userManager.AddToRolesAsync(user, roles);

            return result.Succeeded ? string.Empty : "حدث خطأ ما";
        }
        return "الدور فارغ";
    }

    public Task<List<string>> GetRoles()
    {
        return _roleManager.Roles.Select(x => x.Name).ToListAsync();
    }

    //------------------------------------------------------------------------------------------------------------
    public async Task<IdentityResult> Activate(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المسؤول");

        user.Status = true;
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> Suspend(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المسؤول");

        user.Status = false;
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> DeleteAccountAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("لم يتم العثور على المستخدم");

        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        user.UserName += $"_deleted_{timestamp}";
        user.NormalizedUserName = _userManager.NormalizeName(user.UserName);
        user.IsDeleted = true;

        return await _userManager.UpdateAsync(user);
    }

    public async Task<string> GetUserProfileImage(string profileId)
    {
        if (string.IsNullOrEmpty(profileId))
        {
            return null;
        }

        var profileImage = await _fileHandling.GetFile(profileId);
        return profileImage;
    }

    public async Task<IEnumerable<Specialties>> GetAllSpecialtiesAsync(string userId)
    {
        return await _unitOfWork.SpecialtiesRepository.FindAllAsync(q => q.LawyerId == userId, include: q => q.Include(a => a.mainConsulting));
    }

    public async Task<IEnumerable<Profession>> GetAllProfessionsAsync(string userId)
    {
        return await _unitOfWork.ProfessionsRepository.FindAllAsync(q => q.LawyerId == userId);
    }

    public async Task<IEnumerable<Experience>> GetAllExperiencesAsync(string userId)
    {
        return await _unitOfWork.ExperienceRepository.FindAllAsync(q => q.LawyerId == userId, include: q => q.Include(a => a.subConsulting));
    }

    public async Task<IEnumerable<lawyerLicense>> GetAllLawyerLicensesAsync(string userId)
    {
        return await _unitOfWork.lawyerLicenseRepository.FindAllAsync(q => q.LawyerId == userId);
    }

    public async Task<IEnumerable<graduationCertificate>> GetAllGraduationCertificatesAsync(string userId)
    {
        return await _unitOfWork.graduationCertificateRepository.FindAllAsync(q => q.LawyerId == userId);
    }

    public async Task<List<LawyerDTO>> GetLawyersAsync(
        string? keyword,
        string? city,
        string? specialization,
        int? minYearsExperience,
        int? maxYearsExperience,
        string? sortBy)
    {
        // Get the "Lawyer" role
        var lawyerRole = await _unitOfWork.RoleRepository.FindAsync(r => r.Name == "Lawyer");

        if (lawyerRole == null)
            return new List<LawyerDTO>(); // No lawyer role found

        // Get all user IDs with the "Lawyer" role
        var lawyerUserIds = (await _unitOfWork.UserRoleRepository
            .FindAllAsync(ur => ur.RoleId == lawyerRole.Id))
            .Select(ur => ur.UserId);

        // Start building the filter expression
        Expression<Func<ApplicationUser, bool>> filter = u => lawyerUserIds.Contains(u.Id) && !u.IsDeleted && u.RegistrationStatus.Equals(LawyerRegistrationStatus.Approved) && u.Available;

        // Apply filters dynamically
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var keywordFilter = (Expression<Func<ApplicationUser, bool>>)(u =>
                u.FullName.Contains(keyword) || u.Email.Contains(keyword) || u.Description.Contains(keyword));

            filter = CombineExpressions(filter, keywordFilter);
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            var cityFilter = (Expression<Func<ApplicationUser, bool>>)(u => u.City == city);
            filter = CombineExpressions(filter, cityFilter);
        }

        if (!string.IsNullOrWhiteSpace(specialization))
        {
            var specializationFilter = (Expression<Func<ApplicationUser, bool>>)(u =>
                u.Specialties.Any(s => s.mainConsulting.Name.Contains(specialization)));

            filter = CombineExpressions(filter, specializationFilter);
        }

        if (minYearsExperience.HasValue)
        {
            var minExperienceFilter = (Expression<Func<ApplicationUser, bool>>)(u => u.yearsExperience >= minYearsExperience);
            filter = CombineExpressions(filter, minExperienceFilter);
        }

        if (maxYearsExperience.HasValue)
        {
            var maxExperienceFilter = (Expression<Func<ApplicationUser, bool>>)(u => u.yearsExperience <= maxYearsExperience);
            filter = CombineExpressions(filter, maxExperienceFilter);
        }

        // Sorting logic
        Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> userOrderBy = query => query.OrderBy(u => u.Id);

        if (sortBy is not null)
        {
            userOrderBy = sortBy switch
            {
                "name" => u => u.OrderBy(u => u.FullName),
                "experience" => u => u.OrderByDescending(u => u.yearsExperience),
                "city" => u => u.OrderBy(u => u.City),
                _ => userOrderBy
            };
        }

        // Retrieve filtered users from the repository
        var lawyers = await _unitOfWork.UserRepository.FindAllAsync(
            filter,
            orderBy: userOrderBy
        );

        var lawyerDtos = new List<LawyerDTO>();

        var evaluations = await _unitOfWork.EvaluationRepository
        .FindAllAsync(e => lawyers.Select(l => l.Id).Contains(e.EvaluatedId));

        var evaluationsGrouped = evaluations
            .GroupBy(e => e.EvaluatedId)
            .ToDictionary(g => g.Key, g => g.Average(e => e.Rating));

        var profileImages = await _fileHandling.GetAllFiles(lawyers.Select(l => l.ProfileId).ToList());

        foreach (var lawyer in lawyers)
        {
            var lawyerDto = mapper.Map<LawyerDTO>(lawyer);

            lawyerDto.ProfileImage = profileImages.TryGetValue(lawyer.ProfileId, out var imagePath)
                ? imagePath : null;

            lawyerDto.Rating = evaluationsGrouped.TryGetValue(lawyer.Id, out var averageRating)
                ? Math.Round(averageRating, 1)
                : 0;

            lawyerDtos.Add(lawyerDto);
        }

        return lawyerDtos;
    }

    public async Task<UserNotificationProfileDTO> GetUserNotificationProfileDataAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new ArgumentException("لم يتم العثور على المستخدم");

        UserNotificationProfileDTO userDTO = new UserNotificationProfileDTO()
        {
            Id = user.Id,
            FullName = user.FullName,
            Online = user.Online,
            ProfileImage = await GetUserProfileImage(user.ProfileId)
        };

        return userDTO;
    }

    public async Task<(IdentityResult result, string userId)> RegisterLawyer(RegisterLawyer model)
    {
        if (await IsPhoneExistAsync(model.PhoneNumber, null, true))
        {
            throw new ArgumentException("رقم الهاتف موجود بالفعل");
        }

        if (model.Password != model.ConfirmPassword)
        {
            throw new ArgumentException("كلمة المرور وتأكيدها غير متطابقين");
        }

        if (model.Licenses.Count() < 1 || model.Licenses.Count() > 5)
        {
            throw new ArgumentException("يجب أن تكون ملفات الرخصة بين 1-5");
        }

        if (model.Certificates.Count() < 1 || model.Certificates.Count() > 5)
        {
            throw new ArgumentException("يجب أن تكون الشهادات بين 1-5");
        }

        var user = mapper.Map<ApplicationUser>(model);
        user.RegistrationStatus = LawyerRegistrationStatus.RequestReceived;
        var path = await GetPathByName("ProfileImages");
        user.ProfileId = await _fileHandling.DefaultProfile(path);

        user.UserName = $"{user.UserName}_lawyer";

        var documentsPath = await GetPathByName("lawyerLicense");
        user.Documents = new List<Images>();
        foreach (var file in model.Licenses)
        {
            string fileId = await _fileHandling.UploadFile(file, path);
            user.Documents.Add(await _unitOfWork.ImagesRepository.FindAsync(a => a.Id == fileId));
        }

        documentsPath = await GetPathByName("graduationCertificate");
        foreach (var file in model.Certificates)
        {
            string fileId = await _fileHandling.UploadFile(file, path);
            user.Documents.Add(await _unitOfWork.ImagesRepository.FindAsync(a => a.Id == fileId));
        }

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Lawyer");

            lawyerLicense license = new lawyerLicense()
            {
                LicenseNumber = model.LicenseNumber,
                LawyerId = user.Id,
                State = model.City
            };
            _unitOfWork.lawyerLicenseRepository.Add(license);
            await _unitOfWork.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException($"فشل إنشاء المستخدم: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return (result, user.Id);
    }

    public async Task<(bool IsSuccess, string Token, string ErrorMessage)> LawyerLogin(LoginModel model)
    {
        try
        {
            model.PhoneNumber = $"{model.PhoneNumber}_lawyer";
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password) || user.IsDeleted)
            {
                return (false, null, "محاولة تسجيل دخول غير صالحة");
            }

            if (!user.Status)
            {
                return (false, null, "حسابك معطل. يرجى الاتصال بالدعم");
            }

            if (user.RegistrationStatus == LawyerRegistrationStatus.RequestReceived || user.RegistrationStatus == LawyerRegistrationStatus.DetailSibmitted)
            {
                return (false, null, "حسابك غير معتمد بعد");
            }

            // Proceed with login
            await _signInManager.SignInAsync(user, model.RememberMe);
            var token = await GenerateJwtToken(user, false);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return (true, tokenString, null);
        }
        catch (Exception ex)
        {
            return (false, null, ex.Message);
        }
    }

    public async Task<(bool IsSuccess, string Message)> ChangeLawyerRegistrationStatus(string lawyerId)
    {
        try
        {
            var lawyer = await GetUserById(lawyerId);
            if (lawyer is null)
            {
                return (false, "لم يتم العثور على المحامي");
            }
            else if (lawyer.RegistrationStatus == LawyerRegistrationStatus.NotLawyer)
            {
                return (false, "لا يمكن تغيير حالة حساب العميل");
            }
            else if (lawyer.RegistrationStatus == LawyerRegistrationStatus.RequestReceived)
            {
                lawyer.RegistrationStatus = LawyerRegistrationStatus.LicenseApproved;
                await _userManager.UpdateAsync(lawyer);

                await _userManager.AddToRoleAsync(lawyer, "Notary");

                return (true, "تمت الموافقة على الرخصة");
            }
            else if (lawyer.RegistrationStatus == LawyerRegistrationStatus.LicenseApproved)
            {
                lawyer.RegistrationStatus = LawyerRegistrationStatus.DetailSibmitted;
                await _userManager.UpdateAsync(lawyer);
                return (true, "تم تقديم التفاصيل");
            }
            else if (lawyer.RegistrationStatus == LawyerRegistrationStatus.DetailSibmitted)
            {
                lawyer.RegistrationStatus = LawyerRegistrationStatus.Approved;
                await _userManager.UpdateAsync(lawyer);
                return (true, "تمت الموافقة على الحساب");
            }
            else if (lawyer.RegistrationStatus == LawyerRegistrationStatus.Approved)
            {
                return (true, "حساب المحامي معتمد بالفعل");
            }
            else
            {
                return (false, "حالة غير صالحة");
            }
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<IEnumerable<CityDTO>> GetCitiesAsync()
    {
        var cities = await _unitOfWork.CityRepository.GetAllAsync();
        return mapper.Map<IEnumerable<CityDTO>>(cities);
    }

    private Expression<Func<T, bool>> CombineExpressions<T>(
    Expression<Func<T, bool>> expr1,
    Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));

        var combinedBody = Expression.AndAlso(
            Expression.Invoke(expr1, parameter),
            Expression.Invoke(expr2, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
    }

    //------------------------------------------------------------------------------------------------------------
    #region create and validate JWT token

    private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user, bool isCustomer)
    {
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("uid", user.Id),
                new Claim("name", user.FullName),
                new Claim(ClaimTypes.Role, isCustomer ? "Customer":"Lawyer"),
                new Claim("profileImage", await _fileHandling.GetFile(user.ProfileId)),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwt.Issuer,
            _jwt.Audience,
            claims,
            expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_jwt.DurationInDays)),
            signingCredentials: creds);
        return token;
    }

    public string ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            if (token == null)
                return null;
            if (token.StartsWith("Bearer "))
                token = token.Replace("Bearer ", "");

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var accountId = jwtToken.Claims.First(x => x.Type == "uid").Value;

            return accountId;
        }
        catch
        {
            return null;
        }
    }

    #endregion create and validate JWT token

    #region Random number and string

    //Generate RandomNo
    public int GenerateRandomNo()
    {
        const int min = 1000;
        const int max = 9999;
        var rdm = new Random();
        return rdm.Next(min, max);
    }

    public string RandomString(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public string RandomOTP(int length)
    {
        var random = new Random();
        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    #endregion Random number and string
}