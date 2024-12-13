using System.Security.Claims;
using AutoMapper;
using CollageSystem.Application.DTOs.Account;
using CollageSystem.Application.Services.Interfaces;
using CollageSystem.Core.Models;
using CollageSystem.Core.Models.SecurityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(UserManager<AppUser> userManager, ILogger<AccountController> logger, IMapper mapper,
    IUserService userService, SignInManager<AppUser> signInManager) : ControllerBase
{
    private static string _token = string.Empty;
    private readonly ILogger<AccountController> _logger = logger;
    
    private readonly SignInManager<AppUser> _signInManager = signInManager;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IUserService _userService = userService;

    [HttpGet("[action]")]
    public IActionResult CheckRoles()
    {
        try
        {
            var user = HttpContext.User;
            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

            return Ok(roles);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message + "\n");
            return StatusCode(500, $"internal error+\n{e.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Register(registerDto);
            if (result.IsSuccess)
                return StatusCode(201, new { token = result.Message });
            return StatusCode(500, new { message = string.Join('\n', result.Errors.Select(x => x.Message)) ,errorLevels = result.Errors.Select(x=>x.Level) });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return StatusCode(500, new { message = $"there is an error occurred{e.Message}" });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!_isValidLoginModel(model))
        {
            return BadRequest("Invalid login model ensure you input correct login credentials");
        }

        var user = await _userManager.FindByEmailAsync(model.Email ?? " ") ??
                   await _userManager.FindByNameAsync(model.Username ?? " ");
        if (user != null && (await _signInManager.CheckPasswordSignInAsync(user, model.Password, false)).Succeeded)
        {
            var token = await _userService.GenerateToken(user: user);
            _token = token;
            return Ok(new UserResponse() { Token = token, UserName = user.UserName ?? user.Email });
        }

        return BadRequest($"Invalid username or password if you not register yet please register and try again.");
    }

    private bool _isValidLoginModel(LoginModel model)
    {
        return !string.IsNullOrEmpty(model.Password) &&
               (!string.IsNullOrEmpty(model.Email) || !string.IsNullOrEmpty(model.Username));
    }

    [HttpGet("getToken")]
    public static Task<string> GetToken()
    {
        return Task.FromResult(_token);
    }
}