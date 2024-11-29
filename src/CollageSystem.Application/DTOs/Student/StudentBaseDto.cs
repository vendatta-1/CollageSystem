using System.ComponentModel.DataAnnotations;

namespace CollageSystem.Application.DTOs.Student;

public class StudentBaseDto
{
    [Required] public string FirstName { get; set; }

    public string? LastName { get; set; }
}