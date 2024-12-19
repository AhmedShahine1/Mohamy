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
using Microsoft.AspNetCore.Authorization;
using Mohamy.Core.DTO.AuthViewModel.UpdateModel;
using Mohamy.Core.Entity.LawyerData;
using Microsoft.EntityFrameworkCore.Query;
using Mohamy.Core.DTO.AuthViewModel.LawyerDetailsModel;
using System.Linq.Expressions;

namespace Mohamy.BusinessLayer.Services;

public class AccountService : IAccountService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileHandling _fileHandling;
    private readonly Jwt _jwt;
    private readonly IMemoryCache memoryCache;
    private readonly IMapper mapper;

    public AccountService(UserManager<ApplicationUser> userManager, IFileHandling photoHandling,
        RoleManager<ApplicationRole> roleManager, IUnitOfWork unitOfWork,
        IOptions<Jwt> jwt, IMemoryCache _memoryCache, IMapper _mapper, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _jwt = jwt.Value;
        _fileHandling = photoHandling;
        memoryCache = _memoryCache;
        mapper = _mapper;
        _signInManager = signInManager;
    }
    //------------------------------------------------------------------------------------------------------------
    public async Task<ApplicationUser> GetUserById(string id)
    {
        var user = await _userManager.Users
            .Include(u => u.Profile)
            //.Include(u=>u.Experiences)
            .FirstOrDefaultAsync(x => x.Id == id && x.Status);
        return user;
    }
    //------------------------------------------------------------------------------------------------------------
    // Check if email or phone number already exists before creating or updating the user
    private async Task<bool> IsPhoneExistAsync(string phoneNumber, string userId = null)
    {
        return await _userManager.Users.AnyAsync(u =>
            (u.PhoneNumber == phoneNumber) && u.Id != userId);
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
                throw new InvalidOperationException("Failed to update profile image", ex);
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
                throw new ArgumentException("User not found");
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
        throw new InvalidOperationException("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    return result;
}

    public async Task<IdentityResult> RegisterCustomer(RegisterCustomer model)
    {
        if (await IsPhoneExistAsync(model.PhoneNumber))
        {
            throw new ArgumentException("phone number already exists.");
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
            throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return result;
    }

    public async Task<IdentityResult> RegisterAdmin(RegisterAdmin model)
    {
        if (await IsPhoneExistAsync(model.PhoneNumber))
        {
            throw new ArgumentException("phone number already exists.");
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
            throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return result;
    }

    //------------------------------------------------------------------------------------------------------------
    public async Task<IdentityResult> UpdateAdmin(string adminId, RegisterAdmin model)
    {
        var user = await _userManager.FindByIdAsync(adminId);
        if (user == null)
            throw new ArgumentException("Admin not found");

        if (await IsPhoneExistAsync(model.PhoneNumber, adminId))
        {
            throw new ArgumentException("phone number already exists.");
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
            throw new ArgumentException("Admin not found");

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
                Console.WriteLine($"Error updating file: {ex.Message}");
                throw new InvalidOperationException("Failed to update profile image", ex);
            }
        }

        try
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // Log errors
                Console.WriteLine($"Error updating user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                throw new InvalidOperationException($"Error updating user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            return result;
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error updating user: {ex.Message}");
            throw new InvalidOperationException("Failed to update admin", ex);
        }
    }

    public async Task<IdentityResult> UpdateCustomer(string customerId, UpdateCustomer model)
    {
        var user = await _userManager.FindByIdAsync(customerId);
        if (user == null)
            throw new ArgumentException("Customer not found");

        if (await IsPhoneExistAsync(model.PhoneNumber, customerId))
        {
            throw new ArgumentException("phone number already exists.");
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
            throw new InvalidOperationException("User not found");

        // Validate password and confirmation (already done by Data Annotations)
        if (updatePasswordModel.Password != updatePasswordModel.ConfirmPassword)
            throw new ArgumentException("Password and Confirm Password do not match");

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
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return (false, null, "Invalid login attempt.");
            }

            // Check if user status is true and phone number is confirmed
            if (!user.Status)
            {
                return (false, null, "Your account is deactivated. Please contact support.");
            }

            if (!user.PhoneNumberConfirmed)
            {
                return (false, null, "Phone number not confirmed. Please verify your phone number.");
            }

            // Proceed with login
            await _signInManager.SignInAsync(user, model.RememberMe);
            var token = await GenerateJwtToken(user);
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
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5), // Cache for 5 minutes
            SlidingExpiration = TimeSpan.FromMinutes(2) // Reset cache if accessed within 2 minutes
        };
        var OTP = RandomOTP(6);
        memoryCache.Set(customerPhoneNumber, OTP, cacheEntryOptions);
        return true;
    }

    public async Task<bool> ValidateOTP(string customerPhoneNumber , string OTPV)
    {
        memoryCache.TryGetValue(customerPhoneNumber, out string OTP);
        //if(OTP == null) return false;
        //if(OTP != OTPV) return false;
        return true;
    }

    //------------------------------------------------------------------------------------------------------------

    public async Task<Paths> GetPathByName(string name)
    {
        var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
        if (path == null) throw new ArgumentException("Path not found");
        return path;
    }
    //------------------------------------------------------------------------------------------------------------
    public async Task<string> AddRoleAsync(RoleUserModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user is null)
            return "Vendor not found!";

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

            return result.Succeeded ? string.Empty : "Something went wrong";
        }
        return " Role is empty";
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
            throw new ArgumentException("Admin not found");

        user.Status = true;
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> Suspend(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("Admin not found");

        user.Status = false;
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
        return await _unitOfWork.SpecialtiesRepository.FindAllAsync(q => q.LawyerId == userId, include: q => q.Include(a => a.subConsulting));
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

    public async Task<List<AuthDTO>> SearchLawyersAsync(
        string keyword,
        string city,
        string specialization,
        int? minYearsExperience,
        int? maxYearsExperience,
        string sortBy)
    {
        // Get the "Lawyer" role
        var lawyerRole = await _unitOfWork.RoleRepository.FindAsync(r => r.Name == "Lawyer");

        if (lawyerRole == null)
            return new List<AuthDTO>(); // No lawyer role found

        // Get all user IDs with the "Lawyer" role
        var lawyerUserIds = (await _unitOfWork.UserRoleRepository
            .FindAllAsync(ur => ur.RoleId == lawyerRole.Id))
            .Select(ur => ur.UserId);

        // Start building the filter expression
        Expression<Func<ApplicationUser, bool>> filter = u => lawyerUserIds.Contains(u.Id);

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
                u.Specialties.Any(s => s.subConsulting.Name.Contains(specialization)));

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
        Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> userOrderBy = null;

        if (sortBy != null)
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

        var lawyerDtos = new List<AuthDTO>();

        foreach (var lawyer in lawyers)
        {
            var lawyerDto = mapper.Map<AuthDTO>(lawyer);

            // Populate ProfileImage
            lawyerDto.ProfileImage = await _fileHandling.GetFile(lawyer.ProfileId);

            // Populate specialties
            lawyerDto.Specialties = mapper.Map<List<SpecialtiesDTO>>(await GetAllSpecialtiesAsync(lawyerDto.Id));

            lawyerDtos.Add(lawyerDto);
        }

        return lawyerDtos;
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

    private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("uid", user.Id),
                new Claim("name", user.FullName),
                new Claim(ClaimTypes.Role, "Customer"),
                new Claim("profileImage", await _fileHandling.GetFile(user.ProfileId)),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwt.Issuer,
            _jwt.Audience,
            claims,
            expires: DateTime.Now.AddDays(Convert.ToDouble(_jwt.DurationInDays)),
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