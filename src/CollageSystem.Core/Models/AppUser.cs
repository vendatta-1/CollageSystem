using Microsoft.AspNetCore.Identity;


namespace CollageSystem.Core.Models;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
}