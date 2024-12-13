
using CollageSystem.Application.DTOs.Account;
using CollageSystem.Core.Models;
using CollageSystem.Core.Results;

namespace CollageSystem.Application.Services.Interfaces;


public interface IUserService
{
    Task<string> GenerateToken(AppUser user);
    Task<OperationResult> CreateAdmin(AppUser user, string password);
    Task<OperationResult> CreateUser(AppUser user, string password);
    Task<OperationResult> CreateSuperUser(AppUser user, string password);
    Task<OperationResult> CreateStudent(AppUser user, string password);
    Task<OperationResult> Register(RegisterDto registerModel);
    Task<OperationResult> Login(LoginModel  loginModel);
}