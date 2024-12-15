
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using CollageSystem.Application.DTOs.Account;
using CollageSystem.Application.Services.Interfaces;
using CollageSystem.Core.Models;
using CollageSystem.Core.Models.SecurityModels;
using CollageSystem.Core.Results;
using CollageSystem.Core.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static CollageSystem.Core.Validation.ErrorCode;

namespace CollageSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly Jwt _jwt;
        private readonly ILogger<OperationResult> _operationLogger;
        private readonly OperationResult _result;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;


        public UserService(UserManager<AppUser> userManager, IOptions<Jwt> options,
            ILogger<OperationResult> operationLogger, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _jwt = options.Value;
            _operationLogger = operationLogger;
            _result = new OperationResult(OperationStatus.Pending, _operationLogger);
            _roleManager = roleManager;
            _mapper = mapper;
        }


        public async Task<string> GenerateToken(AppUser user)
        {
            try
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var userClaims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.GivenName, user.UserName ?? ""),
                    new(JwtRegisteredClaimNames.Email, user.Email ?? "")
                };

                var rolesClaim = userRoles.Select(x => new Claim(ClaimTypes.Role, x));

                userClaims.AddRange(rolesClaim);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(userClaims),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = signIn,
                    Audience = _jwt.Audience,
                    Issuer = _jwt.Issuer
                };

                var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception e)
            {
                _operationLogger.LogError(e, "Error generating token");
                return string.Empty;
            }
        }

        public async Task<OperationResult> CreateAdmin(AppUser user, string password)
        {
            return await _CreateRoleBasedUser(user, AppRoles.Administrator, password);
        }

        public async Task<OperationResult> CreateUser(AppUser user, string password)
        {
            return await _CreateRoleBasedUser(user, AppRoles.User, password);
        }

        public async Task<OperationResult> CreateSuperUser(AppUser user, string password)
        {
            return await _CreateRoleBasedUser(user, AppRoles.SuperUser, password);
        }

        public async Task<OperationResult> CreateStudent(AppUser user, string password)
        {
            return await _CreateRoleBasedUser(user, AppRoles.Student, password);
        }

        public async Task<OperationResult> Register(RegisterDto registerModel)
        {
            try
            {
                var appUser = _mapper.Map<AppUser>(registerModel);

                var result = await _userManager.CreateAsync(appUser, registerModel.Password ?? "password");
                if (!result.Succeeded)
                    throw new OperationCanceledException("the creation of the user is field");

                var roleResult = await _userManager.AddToRoleAsync(appUser, AppRoles.User);

                var roles = await _userManager.GetRolesAsync(appUser);



                if (!roleResult.Succeeded)
                {
                    throw new OperationCanceledException(
                        "there is some error while try to add the role to the user");
                }


                _operationLogger.LogInformation("User created a new account with password.");
                var token = await GenerateToken(user: appUser);

                _result.Message = token;

                return _result;


            }
            catch (Exception e)
            {
                _operationLogger.Log(logLevel: LogLevel.Critical, message: e.Message);
                return  _result.WithException(e).WithStatus(OperationStatus.Failure).WithErrorCode(CreateFailed);
            }


        }

        public Task<OperationResult> Login(LoginModel loginModel)
        {
            throw new NotImplementedException();
        }

        private async Task<OperationResult> _CreateRoleBasedUser(AppUser user, string roleName, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(user.UserName))
                    return _result.WithStatus(OperationStatus.Failure).WithErrorCode(CreateFailed,
                        customMessage: "the username cannot be null", FailureLevel.Critical);

                if (_IsExists(user.UserName).Result)
                    return _result.WithStatus(OperationStatus.Failure).WithErrorCode(CreateFailed,
                        customMessage: "the user already exists", FailureLevel.Minor);


                if (!_Create(user, password).Result)
                    return _result.WithStatus(OperationStatus.Failure).WithErrorCode(CreateFailed);

                return await _AddRole(user, roleName);
                // var assignResult = await _userManager.AddToRoleAsync(user, roleName);
                //
                // return assignResult.Succeeded
                //     ? _result.WithStatus(OperationStatus.Success)
                //     : _result.WithStatus(OperationStatus.Failure).WithErrorCode(CreateFailed, "All operations succeeded, but adding to role failed. Check internet or database.", FailureLevel.Minor);
                //
            }
            catch (Exception e)
            {
                _operationLogger.LogError(e,
                    $"Error in private create method while trying to create user of type{roleName}",
                    FailureLevel.Minor);
                return _result.WithStatus(OperationStatus.Failure).WithException(e);
            }
        }

        private async Task<bool> _IsExists(string userName)
        {
            return await _userManager.FindByNameAsync(userName) is not null;
        }

        private async Task<bool> _Create(AppUser user, string password)
        {
            return (await _userManager.CreateAsync(user, password)).Succeeded;
        }

        private async Task<OperationResult> _AddRole(AppUser user, string roleName)
        {
            try
            {
                if (!await _IsRoleEnabled(roleName))
                    return _result.WithStatus(OperationStatus.Failure).WithErrorCode(CreateFailed,
                        $"The role '{roleName}' does not exist or is disabled.", FailureLevel.Critical);
                var addRoleResult = await _userManager.AddToRoleAsync(user, roleName);
                return addRoleResult.Succeeded
                    ? _result.WithStatus(OperationStatus.Success)
                    : _result.WithStatus(OperationStatus.Failure).WithErrorCode(CreateFailed,
                        "Adding to role failed. Check internet or database.", FailureLevel.Minor);
            }
            catch (Exception e)
            {
                _operationLogger.LogError(e, $"Error in private add role method");
                return _result.WithStatus(OperationStatus.Failure).WithException(e);
            }
        }

        private async Task<bool> _IsRoleEnabled(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}