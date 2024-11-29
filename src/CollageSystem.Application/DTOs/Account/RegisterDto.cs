using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CollageSystem.Application.DTOs.Account;

public class RegisterDto
{
    [Required] public string? UserName { get; set; }

    [EmailAddress, Required] public string? Email { get; set; }

    [Required, PasswordPropertyText, DataType(DataType.Password)]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Confirm password does not match!")]
    public string? ConfirmPassword { get; set; }
}